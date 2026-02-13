import http from 'k6/http';
import { check, group, sleep } from 'k6';
import { Counter, Rate, Trend } from 'k6/metrics';

// ─── Métricas personalizadas ────────────────────────────────────────────────
const errorRate = new Rate('errors');
const queryDuration = new Trend('graphql_query_duration', true);
const mutationDuration = new Trend('graphql_mutation_duration', true);
const requestCounter = new Counter('total_requests');

// Trend individual por cada endpoint para ranking de latencia
const endpointTrends = {};
const ENDPOINTS = [
  'health', 'version',
  'allLogs', 'logById', 'logWithDetails', 'logsByFilter', 'failedLogs',
  'logMicroservicesByLogId', 'searchLogMicroservices',
  'logContentsByLogId', 'searchLogContents',
  'allMicroservices', 'activeMicroservices', 'microservicesByClusterId',
  'allClusters', 'activeClusters',
  'allUsers', 'activeUsers',
  'allActivityLogs', 'activityLogsByUser', 'activityLogsByEntity',
  'allEventTypes', 'allCredentials', 'allConnectors', 'connectorsByMicroserviceId',
  'ping',
  'createLogServicesHeader', 'bulkCreateLogServicesHeader', 'updateLogServicesHeader',
  'createLogMicroservice', 'bulkCreateLogMicroservice', 'createLogServicesContent',
  'createCluster', 'updateCluster', 'setClusterActive',
  'createMicroservice', 'updateMicroservice', 'setMicroserviceActive',
  'createUser', 'updateUser', 'setUserActive', 'createActivityLog',
  'createEventType', 'updateEventType',
  'createCredential', 'updateCredential', 'createConnector',
];
for (const name of ENDPOINTS) {
  endpointTrends[name] = new Trend(`ep_${name}`, true);
}

// ─── Configuración ─────────────────────────────────────────────────────────
const BASE_URL = __ENV.GRAPHQL_URL || 'http://localhost:64707/graphql';

export const options = {
  stages: [
    { duration: '30s', target: 50 },
    { duration: '2m',  target: 50 },
    { duration: '30s', target: 0 },
  ],
  thresholds: {
    http_req_duration: ['p(95)<5000'],
    errors: ['rate<0.3'],
  },
};

// ─── Helpers ────────────────────────────────────────────────────────────────
const headers = { 'Content-Type': 'application/json' };

function gql(query, variables = {}) {
  const payload = JSON.stringify({ query, variables });
  const res = http.post(BASE_URL, payload, { headers });
  requestCounter.add(1);
  return res;
}

function checkGql(res, name, isMutation = false) {
  let body;
  try {
    body = res.json();
  } catch (e) {
    errorRate.add(true);
    return {};
  }
  const ok = check(res, {
    [`${name} - status 200`]: (r) => r.status === 200,
    [`${name} - sin errores GraphQL`]: () => !body.errors,
  });
  errorRate.add(!ok);
  if (isMutation) {
    mutationDuration.add(res.timings.duration);
  } else {
    queryDuration.add(res.timings.duration);
  }
  // Registrar en trend individual del endpoint
  if (endpointTrends[name]) {
    endpointTrends[name].add(res.timings.duration);
  }
  return body;
}

// ─── Datos de prueba ────────────────────────────────────────────────────────
function randomString(len) {
  const chars = 'abcdefghijklmnopqrstuvwxyz0123456789';
  let s = '';
  for (let i = 0; i < len; i++) s += chars[Math.floor(Math.random() * chars.length)];
  return s;
}

function randomGuid() {
  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (c) => {
    const r = (Math.random() * 16) | 0;
    return (c === 'x' ? r : (r & 0x3) | 0x8).toString(16);
  });
}

function nowIso() {
  return new Date().toISOString();
}

function pastIso(daysAgo) {
  const d = new Date();
  d.setDate(d.getDate() - daysAgo);
  return d.toISOString();
}

// ═══════════════════════════════════════════════════════════════════════════
//  QUERIES (nombres reales del schema GraphQL)
// ═══════════════════════════════════════════════════════════════════════════

function queryHealth() {
  const res = gql(`{ health }`);
  checkGql(res, 'health');
}

function queryVersion() {
  const res = gql(`{ version }`);
  checkGql(res, 'version');
}

function queryAllLogs() {
  const res = gql(`
    query {
      allLogs(pagination: { pageNumber: 1, pageSize: 10 }) {
        items {
          logId logDateIn logDateOut logState logMethodUrl
          httpMethod microserviceName requestDuration
        }
        totalCount pageNumber pageSize totalPages hasNextPage hasPreviousPage
      }
    }
  `);
  const body = checkGql(res, 'allLogs');
  if (body.data && body.data.allLogs && body.data.allLogs.items && body.data.allLogs.items.length > 0) {
    return body.data.allLogs.items[0].logId;
  }
  return null;
}

function queryLogById(logId) {
  if (!logId) return;
  const res = gql(`
    query($logId: Long!) {
      logById(logId: $logId) {
        logId logDateIn logDateOut logState logMethodUrl
        logMethodName errorCode errorDescription httpMethod microserviceName
      }
    }
  `, { logId });
  checkGql(res, 'logById');
}

function queryLogWithDetails(logId) {
  if (!logId) return;
  const res = gql(`
    query($logId: Long!) {
      logWithDetails(logId: $logId) {
        logId logDateIn logDateOut logState logMethodUrl microserviceName
      }
    }
  `, { logId });
  checkGql(res, 'logWithDetails');
}

function queryLogsByFilter() {
  const res = gql(`
    query($filter: LogFilterInput!, $pagination: PaginationInput) {
      logsByFilter(filter: $filter, pagination: $pagination) {
        items { logId logState logMethodUrl httpMethod microserviceName }
        totalCount pageNumber pageSize
      }
    }
  `, {
    filter: { startDate: pastIso(30), endDate: nowIso() },
    pagination: { pageNumber: 1, pageSize: 10 },
  });
  checkGql(res, 'logsByFilter');
}

function queryFailedLogs() {
  const res = gql(`
    query($fromDate: DateTime) {
      failedLogs(fromDate: $fromDate) {
        logId logState errorCode errorDescription logMethodUrl
      }
    }
  `, { fromDate: pastIso(7) });
  checkGql(res, 'failedLogs');
}

function queryLogMicroservicesByLogId(logId) {
  if (!logId) return;
  const res = gql(`
    query($logId: Long!) {
      logMicroservicesByLogId(logId: $logId) {
        logId logMicroserviceText logDate logLevel
      }
    }
  `, { logId });
  checkGql(res, 'logMicroservicesByLogId');
}

function querySearchLogMicroservices() {
  const res = gql(`
    query($searchText: String!) {
      searchLogMicroservices(searchText: $searchText) {
        logId logMicroserviceText
      }
    }
  `, { searchText: 'k6' });
  checkGql(res, 'searchLogMicroservices');
}

function queryLogContentsByLogId(logId) {
  if (!logId) return;
  const res = gql(`
    query($logId: Long!) {
      logContentsByLogId(logId: $logId) {
        logId logServicesContentText logServicesLogLevel logServicesState
      }
    }
  `, { logId });
  checkGql(res, 'logContentsByLogId');
}

function querySearchLogContents() {
  const res = gql(`
    query($searchText: String!) {
      searchLogContents(searchText: $searchText) {
        logId logServicesContentText logServicesLogLevel
      }
    }
  `, { searchText: 'k6' });
  checkGql(res, 'searchLogContents');
}

function queryAllMicroservices() {
  const res = gql(`
    query {
      allMicroservices {
        microserviceId microserviceName microserviceActive
        microserviceCoreConnection microserviceClusterId
      }
    }
  `);
  const body = checkGql(res, 'allMicroservices');
  if (body.data && body.data.allMicroservices && body.data.allMicroservices.length > 0) {
    return body.data.allMicroservices[0].microserviceId;
  }
  return null;
}

function queryActiveMicroservices() {
  const res = gql(`
    query {
      activeMicroservices {
        microserviceId microserviceName microserviceActive
      }
    }
  `);
  checkGql(res, 'activeMicroservices');
}

function queryMicroservicesByClusterId(clusterId) {
  if (!clusterId) return;
  const res = gql(`
    query($clusterId: Long!) {
      microservicesByClusterId(clusterId: $clusterId) {
        microserviceId microserviceName microserviceActive
      }
    }
  `, { clusterId });
  checkGql(res, 'microservicesByClusterId');
}

function queryAllClusters() {
  const res = gql(`
    query {
      allClusters {
        microservicesClusterId microservicesClusterName
        microservicesClusterServerName microservicesClusterServerIp
        microservicesClusterActive
      }
    }
  `);
  const body = checkGql(res, 'allClusters');
  if (body.data && body.data.allClusters && body.data.allClusters.length > 0) {
    return body.data.allClusters[0].microservicesClusterId;
  }
  return null;
}

function queryActiveClusters() {
  const res = gql(`
    query {
      activeClusters {
        microservicesClusterId microservicesClusterName microservicesClusterActive
      }
    }
  `);
  checkGql(res, 'activeClusters');
}

function queryAllUsers() {
  const res = gql(`
    query {
      allUsers {
        userId userName userEmail userPeoplesoft userActive
      }
    }
  `);
  const body = checkGql(res, 'allUsers');
  if (body.data && body.data.allUsers && body.data.allUsers.length > 0) {
    return body.data.allUsers[0].userId;
  }
  return null;
}

function queryActiveUsers() {
  const res = gql(`
    query {
      activeUsers {
        userId userName userEmail userActive
      }
    }
  `);
  checkGql(res, 'activeUsers');
}

function queryAllActivityLogs() {
  const res = gql(`
    query {
      allActivityLogs {
        activityLogId activityLogEntityName activityLogDescription createAt
      }
    }
  `);
  checkGql(res, 'allActivityLogs');
}

function queryActivityLogsByUser(userId) {
  if (!userId) return;
  const res = gql(`
    query($userId: UUID!) {
      activityLogsByUser(userId: $userId) {
        activityLogId activityLogDescription activityLogEntityName
      }
    }
  `, { userId });
  checkGql(res, 'activityLogsByUser');
}

function queryActivityLogsByEntity() {
  const res = gql(`
    query($entityName: String!) {
      activityLogsByEntity(entityName: $entityName) {
        activityLogId activityLogDescription activityLogEntityName
      }
    }
  `, { entityName: 'MicroserviceRegister' });
  checkGql(res, 'activityLogsByEntity');
}

function queryAllEventTypes() {
  const res = gql(`
    query {
      allEventTypes {
        eventTypeId eventTypeDescription
      }
    }
  `);
  checkGql(res, 'allEventTypes');
}

function queryAllCredentials() {
  const res = gql(`
    query {
      allCredentials {
        coreConnectorCredentialId coreConnectorCredentialUser
      }
    }
  `);
  checkGql(res, 'allCredentials');
}

function queryAllConnectors() {
  const res = gql(`
    query {
      allConnectors {
        microserviceCoreConnectorId coreConnectorCredentialId microserviceId
      }
    }
  `);
  checkGql(res, 'allConnectors');
}

function queryConnectorsByMicroserviceId(msId) {
  if (!msId) return;
  const res = gql(`
    query($microserviceId: Long!) {
      connectorsByMicroserviceId(microserviceId: $microserviceId) {
        microserviceCoreConnectorId coreConnectorCredentialId microserviceId
      }
    }
  `, { microserviceId: msId });
  checkGql(res, 'connectorsByMicroserviceId');
}

// ═══════════════════════════════════════════════════════════════════════════
//  MUTATIONS (nombres reales del schema GraphQL)
// ═══════════════════════════════════════════════════════════════════════════

function mutationPing() {
  const res = gql(`mutation { ping }`);
  checkGql(res, 'ping', true);
}

function mutationCreateLogServicesHeader() {
  const now = nowIso();
  const res = gql(`
    mutation($input: CreateLogServicesHeaderInput!) {
      createLogServicesHeader(input: $input) {
        logId logState logMethodUrl
      }
    }
  `, {
    input: {
      logDateIn: now,
      logDateOut: now,
      logState: 'COMPLETED',
      logMethodUrl: `/api/k6-stress/${randomString(8)}`,
      logMethodName: `k6_test_${randomString(5)}`,
      httpMethod: 'POST',
      microserviceName: 'k6-stress-test',
      transactionId: randomGuid(),
      userId: 'k6-user',
    },
  });
  const body = checkGql(res, 'createLogServicesHeader', true);
  if (body.data && body.data.createLogServicesHeader) {
    return body.data.createLogServicesHeader.logId;
  }
  return null;
}

function mutationBulkCreateLogServicesHeader() {
  const now = nowIso();
  const items = [];
  for (let i = 0; i < 5; i++) {
    items.push({
      logDateIn: now,
      logDateOut: now,
      logState: 'COMPLETED',
      logMethodUrl: `/api/k6-bulk/${randomString(6)}`,
      logMethodName: `k6_bulk_${i}`,
      httpMethod: 'GET',
      microserviceName: 'k6-bulk-test',
      transactionId: randomGuid(),
      userId: 'k6-bulk-user',
    });
  }
  const res = gql(`
    mutation($input: BulkCreateLogServicesHeaderInput!) {
      bulkCreateLogServicesHeader(input: $input) {
        totalRequested totalInserted totalFailed success errorMessage
        insertedItems { logId logMethodUrl }
        errors { index errorMessage }
      }
    }
  `, { input: { items } });
  checkGql(res, 'bulkCreateLogServicesHeader', true);
}

function mutationUpdateLogServicesHeader(logId) {
  if (!logId) return;
  const res = gql(`
    mutation($input: UpdateLogServicesHeaderInput!) {
      updateLogServicesHeader(input: $input) {
        logId logState errorCode
      }
    }
  `, {
    input: {
      logId: logId,
      logState: 'FAILED',
      errorCode: 'K6_ERR',
      errorDescription: 'Stress test update',
      requestDuration: Math.floor(Math.random() * 5000),
    },
  });
  checkGql(res, 'updateLogServicesHeader', true);
}

function mutationCreateLogMicroservice(logId) {
  if (!logId) return;
  const res = gql(`
    mutation($input: CreateLogMicroserviceInput!) {
      createLogMicroservice(input: $input) {
        logId logMicroserviceText
      }
    }
  `, {
    input: {
      logId: logId,
      logDate: nowIso(),
      logLevel: 'INFO',
      logMicroserviceText: `k6 stress test log ${randomString(10)}`,
    },
  });
  checkGql(res, 'createLogMicroservice', true);
}

function mutationBulkCreateLogMicroservice(logId) {
  if (!logId) return;
  const items = [];
  for (let i = 0; i < 5; i++) {
    items.push({
      logId: logId,
      logDate: nowIso(),
      logLevel: ['INFO', 'WARN', 'ERROR'][i % 3],
      logMicroserviceText: `k6 bulk micro log ${i} - ${randomString(8)}`,
    });
  }
  const res = gql(`
    mutation($input: BulkCreateLogMicroserviceInput!) {
      bulkCreateLogMicroservice(input: $input) {
        totalRequested totalInserted totalFailed success
        insertedItems { logId }
      }
    }
  `, { input: { items } });
  checkGql(res, 'bulkCreateLogMicroservice', true);
}

function mutationCreateLogServicesContent(logId) {
  if (!logId) return;
  const res = gql(`
    mutation($input: CreateLogServicesContentInput!) {
      createLogServicesContent(input: $input) {
        logId logServicesContentText
      }
    }
  `, {
    input: {
      logId: logId,
      logServicesDate: nowIso(),
      logServicesLogLevel: 'INFO',
      logServicesState: 'Active',
      logServicesContentText: `k6 content ${randomString(12)}`,
    },
  });
  checkGql(res, 'createLogServicesContent', true);
}

function mutationCreateCluster() {
  const res = gql(`
    mutation($name: String, $serverName: String, $serverIp: String, $active: Boolean!) {
      createCluster(name: $name, serverName: $serverName, serverIp: $serverIp, active: $active) {
        microservicesClusterId microservicesClusterName
      }
    }
  `, {
    name: `k6-cluster-${randomString(5)}`,
    serverName: `k6-server-${randomString(4)}`,
    serverIp: `192.168.${Math.floor(Math.random() * 255)}.${Math.floor(Math.random() * 255)}`,
    active: true,
  });
  const body = checkGql(res, 'createCluster', true);
  if (body.data && body.data.createCluster) {
    return body.data.createCluster.microservicesClusterId;
  }
  return null;
}

function mutationUpdateCluster(clusterId) {
  if (!clusterId) return;
  const res = gql(`
    mutation($id: Long!, $name: String, $active: Boolean) {
      updateCluster(id: $id, name: $name, active: $active) {
        microservicesClusterId microservicesClusterName
      }
    }
  `, { id: clusterId, name: `k6-cl-upd-${randomString(4)}`, active: true });
  checkGql(res, 'updateCluster', true);
}

function mutationSetClusterActive(clusterId) {
  if (!clusterId) return;
  const res = gql(`
    mutation($id: Long!, $active: Boolean!) {
      setClusterActive(id: $id, active: $active)
    }
  `, { id: clusterId, active: true });
  checkGql(res, 'setClusterActive', true);
}

function mutationCreateMicroservice(clusterId) {
  const res = gql(`
    mutation($clusterId: Long, $name: String, $active: Boolean!, $coreConnection: Boolean!) {
      createMicroservice(clusterId: $clusterId, name: $name, active: $active, coreConnection: $coreConnection) {
        microserviceId microserviceName microserviceActive
      }
    }
  `, {
    clusterId: clusterId || null,
    name: `k6-ms-${randomString(6)}`,
    active: true,
    coreConnection: false,
  });
  const body = checkGql(res, 'createMicroservice', true);
  if (body.data && body.data.createMicroservice) {
    return body.data.createMicroservice.microserviceId;
  }
  return null;
}

function mutationUpdateMicroservice(msId) {
  if (!msId) return;
  const res = gql(`
    mutation($id: Long!, $name: String, $active: Boolean) {
      updateMicroservice(id: $id, name: $name, active: $active) {
        microserviceId microserviceName microserviceActive
      }
    }
  `, { id: msId, name: `k6-ms-upd-${randomString(4)}`, active: true });
  checkGql(res, 'updateMicroservice', true);
}

function mutationSetMicroserviceActive(msId) {
  if (!msId) return;
  const res = gql(`
    mutation($id: Long!, $active: Boolean!) {
      setMicroserviceActive(id: $id, active: $active)
    }
  `, { id: msId, active: true });
  checkGql(res, 'setMicroserviceActive', true);
}

function mutationCreateUser() {
  const res = gql(`
    mutation($name: String, $email: String, $peoplesoft: String, $active: Boolean!) {
      createUser(name: $name, email: $email, peoplesoft: $peoplesoft, active: $active) {
        userId userName userEmail
      }
    }
  `, {
    name: `k6-user-${randomString(5)}`,
    email: `k6-${randomString(5)}@test.com`,
    peoplesoft: `PS${randomString(4)}`,
    active: true,
  });
  const body = checkGql(res, 'createUser', true);
  if (body.data && body.data.createUser) {
    return body.data.createUser.userId;
  }
  return null;
}

function mutationUpdateUser(userId) {
  if (!userId) return;
  const res = gql(`
    mutation($id: UUID!, $name: String, $active: Boolean) {
      updateUser(id: $id, name: $name, active: $active) {
        userId userName
      }
    }
  `, { id: userId, name: `k6-usr-upd-${randomString(4)}`, active: true });
  checkGql(res, 'updateUser', true);
}

function mutationSetUserActive(userId) {
  if (!userId) return;
  const res = gql(`
    mutation($id: UUID!, $active: Boolean!) {
      setUserActive(id: $id, active: $active)
    }
  `, { id: userId, active: true });
  checkGql(res, 'setUserActive', true);
}

function mutationCreateActivityLog(userId) {
  if (!userId) return;
  const res = gql(`
    mutation($entityName: String, $entityId: UUID, $description: String, $userId: UUID) {
      createActivityLog(entityName: $entityName, entityId: $entityId, description: $description, userId: $userId) {
        activityLogId activityLogDescription
      }
    }
  `, {
    entityName: 'k6-stress-test',
    entityId: randomGuid(),
    description: `k6 stress test activity ${randomString(8)}`,
    userId: userId,
  });
  checkGql(res, 'createActivityLog', true);
}

function mutationCreateEventType() {
  const res = gql(`
    mutation($description: String!) {
      createEventType(description: $description) {
        eventTypeId eventTypeDescription
      }
    }
  `, { description: `K6_Event_${randomString(6)}` });
  const body = checkGql(res, 'createEventType', true);
  if (body.data && body.data.createEventType) {
    return body.data.createEventType.eventTypeId;
  }
  return null;
}

function mutationUpdateEventType(eventTypeId) {
  if (!eventTypeId) return;
  const res = gql(`
    mutation($id: Long!, $description: String!) {
      updateEventType(id: $id, description: $description) {
        eventTypeId eventTypeDescription
      }
    }
  `, { id: eventTypeId, description: `K6_Upd_${randomString(6)}` });
  checkGql(res, 'updateEventType', true);
}

function mutationCreateCredential() {
  const res = gql(`
    mutation($user: String, $password: String, $key: String) {
      createCredential(user: $user, password: $password, key: $key) {
        coreConnectorCredentialId coreConnectorCredentialUser
      }
    }
  `, {
    user: `k6-cred-${randomString(5)}`,
    password: `k6pass_${randomString(10)}`,
    key: `k6key_${randomString(12)}`,
  });
  const body = checkGql(res, 'createCredential', true);
  if (body.data && body.data.createCredential) {
    return body.data.createCredential.coreConnectorCredentialId;
  }
  return null;
}

function mutationUpdateCredential(credId) {
  if (!credId) return;
  const res = gql(`
    mutation($id: Long!, $user: String) {
      updateCredential(id: $id, user: $user) {
        coreConnectorCredentialId coreConnectorCredentialUser
      }
    }
  `, { id: credId, user: `k6-crd-upd-${randomString(4)}` });
  checkGql(res, 'updateCredential', true);
}

function mutationCreateConnector(credId, msId) {
  if (!credId && !msId) return;
  const res = gql(`
    mutation($credentialId: Long, $microserviceId: Long) {
      createConnector(credentialId: $credentialId, microserviceId: $microserviceId) {
        microserviceCoreConnectorId coreConnectorCredentialId microserviceId
      }
    }
  `, {
    credentialId: credId || null,
    microserviceId: msId || null,
  });
  checkGql(res, 'createConnector', true);
}

// ═══════════════════════════════════════════════════════════════════════════
//  ESCENARIO PRINCIPAL
// ═══════════════════════════════════════════════════════════════════════════

export default function () {
  let existingLogId = null;
  let existingMsId = null;
  let existingClusterId = null;
  let existingUserId = null;

  // ── Health & Version ─────────────────────────────────────────────────
  group('Health & Version', () => {
    queryHealth();
    queryVersion();
  });

  sleep(0.2);

  // ── Log List Queries ─────────────────────────────────────────────────
  group('Log List Queries', () => {
    existingLogId = queryAllLogs();
    queryLogsByFilter();
    queryFailedLogs();
    querySearchLogMicroservices();
    querySearchLogContents();
  });

  sleep(0.2);

  // ── Log Detail Queries (con ID real) ─────────────────────────────────
  group('Log Detail Queries', () => {
    queryLogById(existingLogId);
    queryLogWithDetails(existingLogId);
    queryLogMicroservicesByLogId(existingLogId);
    queryLogContentsByLogId(existingLogId);
  });

  sleep(0.2);

  // ── Microservice List Queries ────────────────────────────────────────
  group('Microservice List Queries', () => {
    existingMsId = queryAllMicroservices();
    queryActiveMicroservices();
    existingClusterId = queryAllClusters();
    queryActiveClusters();
  });

  sleep(0.2);

  // ── Microservice Detail Queries ──────────────────────────────────────
  group('Microservice Detail Queries', () => {
    queryMicroservicesByClusterId(existingClusterId);
    queryConnectorsByMicroserviceId(existingMsId);
  });

  sleep(0.2);

  // ── User & Activity Queries ──────────────────────────────────────────
  group('User & Activity Queries', () => {
    existingUserId = queryAllUsers();
    queryActiveUsers();
    queryAllActivityLogs();
    queryActivityLogsByUser(existingUserId);
    queryActivityLogsByEntity();
    queryAllEventTypes();
  });

  sleep(0.2);

  // ── Credential & Connector Queries ───────────────────────────────────
  group('Credential & Connector Queries', () => {
    queryAllCredentials();
    queryAllConnectors();
  });

  sleep(0.2);

  // ── Ping Mutation ────────────────────────────────────────────────────
  group('Ping Mutation', () => {
    mutationPing();
  });

  sleep(0.2);

  // ── Log Mutations (create → update con ID real) ──────────────────────
  group('Log Mutations', () => {
    const logId = mutationCreateLogServicesHeader();
    mutationUpdateLogServicesHeader(logId);
    mutationCreateLogMicroservice(logId);
    mutationBulkCreateLogMicroservice(logId);
    mutationCreateLogServicesContent(logId);
    mutationBulkCreateLogServicesHeader();
  });

  sleep(0.2);

  // ── Cluster & Microservice Mutations ─────────────────────────────────
  group('Microservice Mutations', () => {
    const clusterId = mutationCreateCluster();
    mutationUpdateCluster(clusterId);
    mutationSetClusterActive(clusterId);

    const msId = mutationCreateMicroservice(clusterId);
    mutationUpdateMicroservice(msId);
    mutationSetMicroserviceActive(msId);
  });

  sleep(0.2);

  // ── User Mutations ───────────────────────────────────────────────────
  group('User Mutations', () => {
    const userId = mutationCreateUser();
    mutationUpdateUser(userId);
    mutationSetUserActive(userId);
    mutationCreateActivityLog(userId);
  });

  sleep(0.2);

  // ── EventType Mutations ──────────────────────────────────────────────
  group('EventType Mutations', () => {
    const eventTypeId = mutationCreateEventType();
    mutationUpdateEventType(eventTypeId);
  });

  sleep(0.2);

  // ── Credential & Connector Mutations ─────────────────────────────────
  group('Credential & Connector Mutations', () => {
    const credId = mutationCreateCredential();
    mutationUpdateCredential(credId);
    mutationCreateConnector(credId, existingMsId);
  });

  sleep(0.3);
}

// ─── Resumen ────────────────────────────────────────────────────────────────
export function handleSummary(data) {
  const W = 74; // ancho interior de la caja
  const line = (text) => `║  ${text.padEnd(W)}║`;
  const sep = `╠${'═'.repeat(W + 2)}╣`;
  const top = `╔${'═'.repeat(W + 2)}╗`;
  const bot = `╚${'═'.repeat(W + 2)}╝`;
  const title = (t) => {
    const pad = Math.max(0, W - t.length);
    const left = Math.floor(pad / 2);
    const right = pad - left;
    return `║  ${' '.repeat(left)}${t}${' '.repeat(right)}║`;
  };

  // ── Recopilar métricas por endpoint ───────────────────────────────────
  const epData = [];
  // Buscar métricas ep_ en todas las métricas disponibles
  for (const [metricName, metricData] of Object.entries(data.metrics)) {
    if (metricName.startsWith('ep_') && metricData.values) {
      const endpointName = metricName.substring(3); // quitar "ep_"
      const avg = metricData.values.avg;
      if (avg === undefined || avg === null) continue; // sin datos
      epData.push({
        name: endpointName,
        avg: avg,
        med: metricData.values.med || 0,
        p95: metricData.values['p(95)'] || 0,
        max: metricData.values.max || 0,
      });
    }
  }

  // Ordenar por avg descendente (más lentos primero)
  epData.sort((a, b) => b.avg - a.avg);

  // ── Construir output ──────────────────────────────────────────────────
  const out = [];
  out.push('');
  out.push(top);
  out.push(title('FastServer GraphQL - Stress Test Results v4'));
  out.push(sep);

  // Resumen general
  const s = data.metrics;
  const fmt = (v) => v !== undefined && v !== null ? `${v.toFixed(0)}ms` : 'N/A';

  out.push(line('RESUMEN GENERAL'));
  out.push(line(`  Total Requests:  ${s.total_requests ? s.total_requests.values.count : 0}`));
  out.push(line(`  Error Rate:      ${s.errors ? (s.errors.values.rate * 100).toFixed(2) + '%' : '0%'}`));
  out.push(line(`  HTTP avg/p95:    ${s.http_req_duration ? fmt(s.http_req_duration.values.avg) + ' / ' + fmt(s.http_req_duration.values['p(95)']) : 'N/A'}`));
  out.push(line(`  Query avg/p95:   ${s.graphql_query_duration ? fmt(s.graphql_query_duration.values.avg) + ' / ' + fmt(s.graphql_query_duration.values['p(95)']) : 'N/A'}`));
  out.push(line(`  Mutation avg/p95:${s.graphql_mutation_duration ? ' ' + fmt(s.graphql_mutation_duration.values.avg) + ' / ' + fmt(s.graphql_mutation_duration.values['p(95)']) : ' N/A'}`));
  out.push(line(`  VUs Max:         ${s.vus_max ? s.vus_max.values.max : 0}`));
  out.push(sep);

  // Ranking completo
  const hdr = `  ${'#'.padStart(3)}  ${'Endpoint'.padEnd(30)} ${'Avg'.padStart(8)} ${'Med'.padStart(8)} ${'P95'.padStart(8)} ${'Max'.padStart(8)}`;
  out.push(line('RANKING POR LATENCIA (mayor a menor)'));
  out.push(line(''));
  out.push(line(hdr));
  out.push(line(`  ${'─'.repeat(3)}  ${'─'.repeat(30)} ${'─'.repeat(8)} ${'─'.repeat(8)} ${'─'.repeat(8)} ${'─'.repeat(8)}`));

  epData.forEach((ep, i) => {
    const rank = String(i + 1).padStart(3);
    const nm = ep.name.padEnd(30);
    const avg = fmt(ep.avg).padStart(8);
    const med = fmt(ep.med).padStart(8);
    const p95 = fmt(ep.p95).padStart(8);
    const max = fmt(ep.max).padStart(8);
    const marker = i < 5 ? ' << SLOW' : '';
    out.push(line(`  ${rank}  ${nm} ${avg} ${med} ${p95} ${max}${marker}`));
  });

  out.push(sep);

  // Top 5 más lentos destacado
  out.push(line('TOP 5 MAS LENTOS'));
  out.push(line(''));
  const top5 = epData.slice(0, 5);
  top5.forEach((ep, i) => {
    const bar = '█'.repeat(Math.min(40, Math.round(ep.avg / (top5[0].avg || 1) * 40)));
    out.push(line(`  ${i + 1}. ${ep.name}`));
    out.push(line(`     avg=${fmt(ep.avg)}  p95=${fmt(ep.p95)}  max=${fmt(ep.max)}`));
    out.push(line(`     ${bar}`));
  });

  out.push(sep);

  // Top 5 más rápidos
  out.push(line('TOP 5 MAS RAPIDOS'));
  out.push(line(''));
  const bot5 = epData.slice(-5).reverse();
  bot5.forEach((ep, i) => {
    out.push(line(`  ${i + 1}. ${ep.name}  avg=${fmt(ep.avg)}  p95=${fmt(ep.p95)}`));
  });

  out.push(bot);
  out.push('');

  console.log(out.join('\n'));

  // JSON para exportar
  const jsonResult = {
    summary: {
      totalRequests: s.total_requests ? s.total_requests.values.count : 0,
      errorRate: s.errors ? s.errors.values.rate : 0,
      httpAvg: s.http_req_duration ? s.http_req_duration.values.avg : null,
      httpP95: s.http_req_duration ? s.http_req_duration.values['p(95)'] : null,
    },
    endpoints: epData,
  };

  return {
    stdout: JSON.stringify(jsonResult, null, 2),
  };
}
