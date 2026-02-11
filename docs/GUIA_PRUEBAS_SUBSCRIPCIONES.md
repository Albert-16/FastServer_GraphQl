# 📡 Guía de Pruebas de Subscripciones GraphQL

**FastServer - Subscripciones en Tiempo Real**
**Fecha:** Febrero 2024

---

## 📋 Tabla de Contenidos

1. [¿Qué son las Subscripciones?](#qué-son-las-subscripciones)
2. [Subscripciones Disponibles](#subscripciones-disponibles)
3. [Cómo Probar Subscripciones](#cómo-probar-subscripciones)
4. [Ejemplos Prácticos](#ejemplos-prácticos)
5. [Casos de Uso Reales](#casos-de-uso-reales)

---

## 🎯 ¿Qué son las Subscripciones?

Las **subscripciones GraphQL** permiten recibir datos en **tiempo real** cuando ocurren eventos en el servidor, sin necesidad de hacer polling.

### Ventajas

- ✅ **Tiempo Real:** Eventos instantáneos cuando ocurren cambios
- ✅ **Eficiente:** No necesitas hacer requests cada X segundos
- ✅ **Bidireccional:** Comunicación WebSocket persistente
- ✅ **Escalable:** Múltiples clientes pueden suscribirse simultáneamente

### Cómo Funciona

```
Cliente (UI) ────────► Suscripción ────────► Servidor
                      (WebSocket)
                           │
                           │ Evento ocurre
                           │ (create/update/delete)
                           ▼
Cliente recibe ◄──────── Notificación
evento en tiempo real
```

---

## 📡 Subscripciones Disponibles

### 1. Subscripciones de Logs (BD: FastServer_Logs)

| Subscripción | Evento | Descripción |
|--------------|--------|-------------|
| `onLogCreated` | Crear log | Se emite cuando se crea un nuevo log |
| `onLogUpdated` | Actualizar log | Se emite cuando se actualiza un log existente |
| `onLogDeleted` | Eliminar log | Se emite cuando se elimina un log |
| `onLogMicroserviceCreated` | Crear log de microservicio | Nuevo log de microservicio |
| `onLogMicroserviceDeleted` | Eliminar log de microservicio | Log de microservicio eliminado |
| `onLogContentCreated` | Crear contenido de log | Nuevo contenido de log |
| `onLogContentDeleted` | Eliminar contenido de log | Contenido de log eliminado |

### 2. Subscripciones de Microservicios (BD: FastServer)

| Subscripción | Evento | Descripción |
|--------------|--------|-------------|
| `onMicroserviceCreated` | Crear microservicio | Nuevo microservicio registrado |
| `onMicroserviceUpdated` | Actualizar microservicio | Microservicio actualizado |
| `onMicroserviceDeleted` | Eliminar microservicio | Microservicio eliminado |
| `onClusterCreated` | Crear cluster | Nuevo cluster de microservicios |
| `onClusterUpdated` | Actualizar cluster | Cluster actualizado |
| `onClusterDeleted` | Eliminar cluster | Cluster eliminado |
| `onUserCreated` | Crear usuario | Nuevo usuario creado |
| `onUserUpdated` | Actualizar usuario | Usuario actualizado |
| `onUserDeleted` | Eliminar usuario | Usuario eliminado |
| `onActivityLogCreated` | Crear log de actividad | Nueva actividad registrada |
| `onActivityLogDeleted` | Eliminar log de actividad | Actividad eliminada |
| `onCredentialCreated` | Crear credencial | Nueva credencial de conector |
| `onCredentialUpdated` | Actualizar credencial | Credencial actualizada |
| `onCredentialDeleted` | Eliminar credencial | Credencial eliminada |

**Total:** 20+ subscripciones disponibles

---

## 🧪 Cómo Probar Subscripciones

### Método 1: GraphQL IDE (Banana Cake Pop)

**Paso 1:** Abrir GraphQL IDE
```
http://localhost:64707/graphql
```

**Paso 2:** Abrir dos pestañas:
- **Pestaña 1:** Para la subscripción
- **Pestaña 2:** Para disparar eventos (mutations)

**Paso 3:** En Pestaña 1, crear subscripción y hacer clic en ▶️ (Play)

**Paso 4:** En Pestaña 2, ejecutar mutation que dispare el evento

**Paso 5:** Observar evento en tiempo real en Pestaña 1

---

### Método 2: Cliente JavaScript

```javascript
// Crear cliente GraphQL con soporte WebSocket
import { createClient } from 'graphql-ws';

const client = createClient({
  url: 'ws://localhost:64707/graphql',
});

// Suscribirse a eventos
const unsubscribe = client.subscribe(
  {
    query: `
      subscription {
        onLogCreated {
          logId
          logState
          microserviceName
        }
      }
    `,
  },
  {
    next: (data) => {
      console.log('📩 Nuevo log creado:', data);
    },
    error: (error) => {
      console.error('❌ Error:', error);
    },
    complete: () => {
      console.log('✅ Subscripción completada');
    },
  }
);

// Para cancelar subscripción
// unsubscribe();
```

---

## 📝 Ejemplos Prácticos

### Ejemplo 1: Monitorear Creación de Logs

**🔔 Subscripción (Pestaña 1):**
```graphql
subscription MonitorearNuevosLogs {
  onLogCreated {
    logId
    logState
    logMethodUrl
    microserviceName
    requestDuration
    transactionId
    userId
    logDateIn
  }
}
```

**🚀 Disparar Evento (Pestaña 2):**
```graphql
mutation {
  createLogServicesHeader(input: {
    logDateIn: "2024-02-11T15:30:00Z"
    logDateOut: "2024-02-11T15:30:02Z"
    logState: COMPLETED
    logMethodUrl: "/api/payments/process"
    logMethodName: "ProcessPayment"
    microserviceName: "PaymentService"
    httpMethod: "POST"
    requestDuration: 2000
    transactionId: "PAY-2024-001"
    userId: "user123"
  }) {
    logId
  }
}
```

**📩 Evento Recibido en Subscripción:**
```json
{
  "data": {
    "onLogCreated": {
      "logId": 5,
      "logState": "COMPLETED",
      "logMethodUrl": "/api/payments/process",
      "microserviceName": "PaymentService",
      "requestDuration": 2000,
      "transactionId": "PAY-2024-001",
      "userId": "user123",
      "logDateIn": "2024-02-11T15:30:00.000Z"
    }
  }
}
```

---

### Ejemplo 2: Monitorear Actualizaciones de Logs

**🔔 Subscripción:**
```graphql
subscription MonitorearActualizacionesLogs {
  onLogUpdated {
    logId
    logState
    errorCode
    errorDescription
    requestDuration
  }
}
```

**🚀 Disparar Evento:**
```graphql
mutation {
  updateLogServicesHeader(input: {
    logId: 5
    logState: FAILED
    errorCode: "PAY-TIMEOUT"
    errorDescription: "El gateway de pago no respondió en 30 segundos"
  }) {
    logId
    logState
  }
}
```

**📩 Evento Recibido:**
```json
{
  "data": {
    "onLogUpdated": {
      "logId": 5,
      "logState": "FAILED",
      "errorCode": "PAY-TIMEOUT",
      "errorDescription": "El gateway de pago no respondió en 30 segundos",
      "requestDuration": 2000
    }
  }
}
```

---

### Ejemplo 3: Monitorear Registro de Microservicios

**🔔 Subscripción:**
```graphql
subscription MonitorearNuevosMicroservicios {
  onMicroserviceCreated {
    microserviceId
    microserviceName
    microserviceActive
    microserviceCoreConnection
    createAt
  }
}
```

**🚀 Disparar Evento:**
```graphql
mutation {
  createMicroservice(
    name: "EmailService"
    active: true
    coreConnection: false
  ) {
    microserviceId
    microserviceName
  }
}
```

**📩 Evento Recibido:**
```json
{
  "data": {
    "onMicroserviceCreated": {
      "microserviceId": 10,
      "microserviceName": "EmailService",
      "microserviceActive": true,
      "microserviceCoreConnection": false,
      "createAt": "2024-02-11T15:35:00.000Z"
    }
  }
}
```

---

### Ejemplo 4: Monitorear Actividad de Usuarios

**🔔 Subscripción:**
```graphql
subscription MonitorearActividadUsuarios {
  onActivityLogCreated {
    activityLogId
    eventType {
      eventTypeId
      eventTypeDescription
    }
    entityName
    activityLogDescription
    user {
      userId
      userName
      userEmail
    }
    createAt
  }
}
```

**🚀 Disparar Evento:**
```graphql
mutation {
  createActivityLog(
    eventTypeId: 1
    entityName: "Microservice"
    description: "Usuario activó microservicio PaymentService"
    userId: "550e8400-e29b-41d4-a716-446655440000"
  ) {
    activityLogId
  }
}
```

**📩 Evento Recibido:**
```json
{
  "data": {
    "onActivityLogCreated": {
      "activityLogId": "7b8e9f10-1234-5678-90ab-cdef12345678",
      "eventType": {
        "eventTypeId": 1,
        "eventTypeDescription": "Activación"
      },
      "entityName": "Microservice",
      "activityLogDescription": "Usuario activó microservicio PaymentService",
      "user": {
        "userId": "550e8400-e29b-41d4-a716-446655440000",
        "userName": "Admin",
        "userEmail": "admin@banco.com"
      },
      "createAt": "2024-02-11T15:40:00.000Z"
    }
  }
}
```

---

### Ejemplo 5: Monitorear Múltiples Eventos (Agregado)

**🔔 Subscripción Múltiple:**
```graphql
subscription MonitorearTodosLosEventos {
  logs: onLogCreated {
    logId
    logState
    microserviceName
  }

  microservices: onMicroserviceCreated {
    microserviceId
    microserviceName
  }

  activity: onActivityLogCreated {
    activityLogId
    activityLogDescription
  }
}
```

**⚠️ Nota:** GraphQL no soporta múltiples subscripciones root en una sola query. Debes crear subscripciones separadas.

**✅ Alternativa - Múltiples Subscripciones:**
```graphql
# Subscripción 1
subscription { onLogCreated { logId } }

# Subscripción 2
subscription { onMicroserviceCreated { microserviceId } }

# Subscripción 3
subscription { onActivityLogCreated { activityLogId } }
```

---

## 🎯 Casos de Uso Reales

### Caso 1: Dashboard en Tiempo Real

**Escenario:** Mostrar logs en tiempo real en un dashboard de monitoreo

**Implementación:**
```javascript
// React + graphql-ws
import { useSubscription } from '@apollo/client';

function DashboardLogs() {
  const { data, loading } = useSubscription(
    gql`
      subscription {
        onLogCreated {
          logId
          logState
          microserviceName
          requestDuration
        }
      }
    `
  );

  return (
    <div>
      <h2>Logs en Tiempo Real</h2>
      {data && (
        <div className="new-log">
          ✅ Nuevo log: {data.onLogCreated.microserviceName}
          ({data.onLogCreated.logState})
        </div>
      )}
    </div>
  );
}
```

---

### Caso 2: Alertas de Errores

**Escenario:** Notificar al equipo cuando un log falla

**Implementación:**
```javascript
const { data } = useSubscription(
  gql`
    subscription {
      onLogUpdated {
        logId
        logState
        errorCode
        errorDescription
        microserviceName
      }
    }
  `
);

useEffect(() => {
  if (data?.onLogUpdated?.logState === 'FAILED') {
    // Enviar notificación push
    notifyTeam({
      title: '❌ Error en Microservicio',
      message: `${data.onLogUpdated.microserviceName}: ${data.onLogUpdated.errorDescription}`,
      severity: 'high'
    });

    // Enviar email
    sendEmail({
      to: 'ops-team@banco.com',
      subject: `Error: ${data.onLogUpdated.errorCode}`,
      body: data.onLogUpdated.errorDescription
    });
  }
}, [data]);
```

---

### Caso 3: Monitoreo de Nuevos Microservicios

**Escenario:** Actualizar lista de microservicios automáticamente

**Implementación:**
```javascript
function MicroservicesList() {
  const [microservices, setMicroservices] = useState([]);

  // Query inicial
  const { data: initialData } = useQuery(GET_ALL_MICROSERVICES);

  // Subscripción a nuevos
  const { data: newData } = useSubscription(
    gql`
      subscription {
        onMicroserviceCreated {
          microserviceId
          microserviceName
          microserviceActive
        }
      }
    `
  );

  useEffect(() => {
    if (initialData) {
      setMicroservices(initialData.allMicroservices);
    }
  }, [initialData]);

  useEffect(() => {
    if (newData) {
      setMicroservices(prev => [
        newData.onMicroserviceCreated,
        ...prev
      ]);

      // Toast notification
      toast.success(
        `✅ Nuevo microservicio: ${newData.onMicroserviceCreated.microserviceName}`
      );
    }
  }, [newData]);

  return (
    <ul>
      {microservices.map(ms => (
        <li key={ms.microserviceId}>{ms.microserviceName}</li>
      ))}
    </ul>
  );
}
```

---

### Caso 4: Auditoría en Tiempo Real

**Escenario:** Registrar todas las acciones de usuarios en tiempo real

**Implementación:**
```javascript
function AuditLog() {
  const [activities, setActivities] = useState([]);

  const { data } = useSubscription(
    gql`
      subscription {
        onActivityLogCreated {
          activityLogId
          activityLogDescription
          user {
            userName
            userEmail
          }
          createAt
        }
      }
    `
  );

  useEffect(() => {
    if (data) {
      const activity = data.onActivityLogCreated;

      // Agregar a lista
      setActivities(prev => [activity, ...prev].slice(0, 100));

      // Log en consola
      console.log(
        `[${activity.createAt}] ${activity.user.userName}: ${activity.activityLogDescription}`
      );

      // Guardar en storage para auditoría offline
      localStorage.setItem(
        `audit_${activity.activityLogId}`,
        JSON.stringify(activity)
      );
    }
  }, [data]);

  return (
    <div className="audit-log">
      <h2>Auditoría en Tiempo Real</h2>
      {activities.map(activity => (
        <div key={activity.activityLogId} className="activity">
          <span className="timestamp">{activity.createAt}</span>
          <span className="user">{activity.user.userName}</span>
          <span className="description">{activity.activityLogDescription}</span>
        </div>
      ))}
    </div>
  );
}
```

---

## ✅ Checklist de Pruebas

### Pruebas Básicas

- [ ] Subscripción `onLogCreated` recibe eventos al crear logs
- [ ] Subscripción `onLogUpdated` recibe eventos al actualizar logs
- [ ] Subscripción `onLogDeleted` recibe eventos al eliminar logs
- [ ] Subscripción `onMicroserviceCreated` recibe eventos al crear microservicios
- [ ] WebSocket se conecta correctamente en `ws://localhost:64707/graphql`

### Pruebas Avanzadas

- [ ] Múltiples clientes pueden suscribirse simultáneamente
- [ ] Subscripción se reconecta automáticamente si cae la conexión
- [ ] Eventos se reciben en <100ms después de la mutation
- [ ] No hay memory leaks con subscripciones de larga duración
- [ ] Subscripciones funcionan con autenticación (si aplica)

### Pruebas de Estrés

- [ ] 100+ subscripciones concurrentes funcionan sin degradación
- [ ] 1000+ eventos/minuto se procesan correctamente
- [ ] Servidor maneja desconexiones abruptas de clientes

---

## 📊 Métricas de Subscripciones

| Métrica | Valor Esperado |
|---------|----------------|
| **Latencia de eventos** | <100ms |
| **Subscripciones concurrentes** | 1000+ |
| **Eventos por segundo** | 500+ |
| **Tiempo de reconexión** | <2s |
| **Memory overhead por subscripción** | ~1KB |

---

## 🚨 Troubleshooting

### Problema: "WebSocket connection failed"

**Solución:**
1. Verificar que `app.UseWebSockets()` esté en `Program.cs`
2. Verificar que el puerto esté abierto en firewall
3. Probar con `ws://` en lugar de `wss://` en desarrollo

### Problema: "Subscription no recibe eventos"

**Solución:**
1. Verificar que el evento se esté publicando en el servicio
2. Verificar que el topic coincida entre subscripción y publisher
3. Verificar logs del servidor para errores

### Problema: "Connection drops after idle"

**Solución:**
1. Implementar keep-alive en cliente
2. Configurar timeout de WebSocket en servidor
3. Enviar ping periódicos

---

## 📚 Documentación Adicional

- **GraphQL Subscriptions Spec:** https://spec.graphql.org/October2021/#sec-Subscription
- **HotChocolate Subscriptions:** https://chillicream.com/docs/hotchocolate/v13/fetching-data/subscriptions
- **graphql-ws Protocol:** https://github.com/enisdenjo/graphql-ws

---

**✅ ¡Subscripciones configuradas y listas para usar en tiempo real!**

*Última actualización: Febrero 2024*
