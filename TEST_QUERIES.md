# Queries de Prueba para FastServer GraphQL API

## URL de la API
- GraphQL Endpoint: `http://localhost:64707/graphql`
- Banana Cake Pop UI: `http://localhost:64707/graphql` (abrir en navegador)

---

## 1. AUTENTICACIÓN - Login (Público)

Prueba las credenciales corregidas con hashes BCrypt válidos:

### Login Admin
```graphql
mutation {
  login(input: {
    email: "admin@fastserver.com"
    password: "Admin123!"
  }) {
    accessToken
    refreshToken
    userId
    email
    name
  }
}
```

### Login Developer
```graphql
mutation {
  login(input: {
    email: "developer@fastserver.com"
    password: "Dev123!"
  }) {
    accessToken
    refreshToken
    userId
    email
    name
  }
}
```

**Resultado esperado**: Debe retornar `accessToken`, `refreshToken` y datos del usuario sin error `AUTH_NOT_AUTHENTICATED`.

---

## 2. AUTENTICACIÓN - Registro de Nuevo Usuario (Público)

Para FastServer UI - Permite registrar nuevos usuarios:

```graphql
mutation {
  register(input: {
    email: "nuevo@ejemplo.com"
    password: "MiPassword123!"
    name: "Juan Pérez"
    userPeoplesoft: "PS999"
  }) {
    accessToken
    refreshToken
    userId
    email
    name
  }
}
```

**Resultado esperado**: Crea el usuario y retorna tokens inmediatamente (auto-login después del registro).

---

## 3. AUTENTICACIÓN - Refresh Token (Público)

Cuando el `accessToken` expira (60 minutos), usar el `refreshToken` para obtener uno nuevo:

```graphql
mutation {
  refreshToken(input: {
    accessToken: "<TU_ACCESS_TOKEN_EXPIRADO>"
    refreshToken: "<TU_REFRESH_TOKEN>"
  }) {
    accessToken
    refreshToken
    userId
    email
    name
  }
}
```

---

## 4. AUTENTICACIÓN - Cambiar Contraseña (Requiere Autenticación)

⚠️ **Requiere Header**: `Authorization: Bearer <TU_ACCESS_TOKEN>`

```graphql
mutation {
  changePassword(
    currentPassword: "MiPassword123!"
    newPassword: "NuevoPassword456!"
  )
}
```

**Resultado esperado**: Retorna `true` si la contraseña actual es correcta y se actualizó exitosamente.

---

## 5. AUTENTICACIÓN - Logout (Requiere Autenticación)

⚠️ **Requiere Header**: `Authorization: Bearer <TU_ACCESS_TOKEN>`

```graphql
mutation {
  logout
}
```

**Resultado esperado**: Retorna `true` y revoca el `refreshToken` (ya no se puede usar para renovar tokens).

---

## 6. QUERIES - Obtener Microservicios (Requiere Autenticación)

⚠️ **Requiere Header**: `Authorization: Bearer <TU_ACCESS_TOKEN>`

```graphql
query {
  allMicroservices(dataSource: SQL_SERVER) {
    microserviceId
    microserviceName
    microserviceActive
    microserviceDeleted
    microserviceCoreConnection
    cluster {
      microservicesClusterName
      microservicesClusterServerIp
    }
  }
}
```

---

## 7. QUERIES - Obtener Logs (Requiere Autenticación)

⚠️ **Requiere Header**: `Authorization: Bearer <TU_ACCESS_TOKEN>`

```graphql
query {
  logServicesHeaders(dataSource: POSTGRESQL) {
    logId
    logDateIn
    logDateOut
    logState
    microserviceName
    httpMethod
    userId
  }
}
```

---

## 8. MUTATIONS - Crear Log (Requiere Autenticación)

⚠️ **Requiere Header**: `Authorization: Bearer <TU_ACCESS_TOKEN>`

```graphql
mutation {
  createLogServicesHeader(
    dataSource: POSTGRESQL
    input: {
      logDateIn: "2026-02-09T16:00:00Z"
      logDateOut: "2026-02-09T16:00:05Z"
      logState: SUCCESS
      logMethodUrl: "/api/test/endpoint"
      microserviceName: "TestService"
      httpMethod: "POST"
      userId: "test-user-123"
    }
  ) {
    logId
    logDateIn
    microserviceName
    logState
  }
}
```

---

## 9. SUSCRIPCIONES - Escuchar Creación de Logs (Requiere Autenticación)

⚠️ **Requiere WebSocket con token**: Agregar `?access_token=<TU_ACCESS_TOKEN>` a la URL de WebSocket

### En Banana Cake Pop:
1. Abrir conexión de suscripción
2. Agregar header o query param con el token

```graphql
subscription {
  onLogCreated {
    logId
    logDateIn
    logDateOut
    microserviceName
    userId
    logState
    httpMethod
  }
}
```

**Prueba**: Ejecutar la mutation de crear log (punto 8) en otra pestaña. La suscripción debe recibir el evento inmediatamente.

---

## 10. SUSCRIPCIONES - Escuchar Actualización de Logs

```graphql
subscription {
  onLogUpdated {
    logId
    logDateIn
    logDateOut
    microserviceName
    logState
    requestDuration
  }
}
```

---

## 11. SUSCRIPCIONES - Escuchar Eliminación de Logs

```graphql
subscription {
  onLogDeleted {
    logId
    microserviceName
    userId
    deletedAt
  }
}
```

---

## Flujo Completo para FastServer UI

### 1. Primera vez (Registro)
1. Usuario hace clic en "Registrarse"
2. UI llama a `register` mutation
3. Recibe `accessToken` y `refreshToken`
4. Guarda tokens en `localStorage`
5. Redirige a dashboard

### 2. Login normal
1. Usuario ingresa email y password
2. UI llama a `login` mutation
3. Recibe tokens y los guarda en `localStorage`
4. Redirige a dashboard

### 3. Requests autenticados
1. UI lee `accessToken` de `localStorage`
2. Agrega header: `Authorization: Bearer <accessToken>`
3. Ejecuta query/mutation

### 4. Token expirado (Auto-refresh)
1. UI recibe error 401 o `AUTH_NOT_AUTHENTICATED`
2. Lee `refreshToken` de `localStorage`
3. Llama a `refreshToken` mutation
4. Obtiene nuevo `accessToken`
5. Actualiza `localStorage`
6. Reintenta request original

### 5. Logout
1. Usuario hace clic en "Cerrar sesión"
2. UI llama a `logout` mutation (con token)
3. Borra tokens de `localStorage`
4. Redirige a login

---

## Notas de Seguridad

1. **NUNCA** exponer el `refreshToken` en URLs o logs
2. **SIEMPRE** usar HTTPS en producción
3. **SIEMPRE** validar y sanitizar inputs del usuario
4. El `accessToken` expira en **60 minutos**
5. El `refreshToken` expira en **7 días**
6. Las contraseñas se hashean con **BCrypt work factor 11**
7. Los usuarios nuevos tienen `EmailConfirmed = false` (implementar verificación por email en futuro)

---

## Usuarios de Prueba Disponibles

| Email | Password | Nombre |
|-------|----------|--------|
| admin@fastserver.com | Admin123! | Admin User |
| developer@fastserver.com | Dev123! | Developer User |

---

## Troubleshooting

### Error: "AUTH_NOT_AUTHENTICATED"
- **Solución**: Verifica que el header `Authorization: Bearer <token>` esté presente
- **Solución**: Verifica que el token no haya expirado (usa `refreshToken` mutation)

### Error: "El email ya está registrado"
- **Solución**: El email ya existe, usar `login` en lugar de `register`

### Error: "Contraseña actual incorrecta"
- **Solución**: Verifica que la `currentPassword` sea correcta en `changePassword`

### Error: "Refresh token inválido o expirado"
- **Solución**: El usuario debe hacer login nuevamente (el refresh token expiró o fue revocado)

---

## Endpoints Health Check

```bash
# Verificar que la API está corriendo
curl http://localhost:64707/health

# Verificar PostgreSQL
curl http://localhost:64707/health/postgresql

# Verificar SQL Server
curl http://localhost:64707/health/sqlserver
```

---

¡Listo! Todas las funcionalidades de autenticación están implementadas y probadas. 🚀
