IF OBJECT_ID(N'[dbo].[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [core_connector_credentials] (
    [core_connector_credential_id] bigint NOT NULL IDENTITY,
    [core_connector_credential_user] nvarchar(255) NULL,
    [core_connector_credential_pass] nvarchar(500) NULL,
    [core_connector_credential_key] nvarchar(500) NULL,
    [create_at] datetime2 NULL,
    [modify_at] datetime2 NULL,
    CONSTRAINT [PK_core_connector_credentials] PRIMARY KEY ([core_connector_credential_id])
);

CREATE TABLE [event_types] (
    [event_type_id] bigint NOT NULL IDENTITY,
    [event_type_description] nvarchar(500) NULL,
    [create_at] datetime2 NULL,
    [modify_at] datetime2 NULL,
    CONSTRAINT [PK_event_types] PRIMARY KEY ([event_type_id])
);

CREATE TABLE [microservices_clusters] (
    [microservices_cluster_id] bigint NOT NULL IDENTITY,
    [microservices_cluster_name] nvarchar(255) NULL,
    [microservices_cluster_server_name] nvarchar(255) NULL,
    [microservices_cluster_server_ip] nvarchar(50) NULL,
    [microservices_cluster_active] bit NULL,
    [microservices_cluster_deleted] bit NULL,
    [delete_at] datetime2 NULL,
    [create_at] datetime2 NULL,
    [modify_at] datetime2 NULL,
    CONSTRAINT [PK_microservices_clusters] PRIMARY KEY ([microservices_cluster_id])
);

CREATE TABLE [users] (
    [user_id] uniqueidentifier NOT NULL,
    [user_peoplesoft] nvarchar(100) NULL,
    [user_active] bit NULL,
    [user_name] nvarchar(255) NULL,
    [user_email] nvarchar(255) NULL,
    [password_hash] nvarchar(500) NULL,
    [last_login] datetime2 NULL,
    [password_changed_at] datetime2 NULL,
    [email_confirmed] bit NOT NULL DEFAULT CAST(0 AS bit),
    [refresh_token] nvarchar(500) NULL,
    [refresh_token_expiry_time] datetime2 NULL,
    [create_at] datetime2 NULL,
    [modify_at] datetime2 NULL,
    CONSTRAINT [PK_users] PRIMARY KEY ([user_id])
);

CREATE TABLE [microservice_registers] (
    [microservice_id] bigint NOT NULL IDENTITY,
    [microservice_cluster_id] bigint NULL,
    [microservice_name] nvarchar(255) NULL,
    [microservice_active] bit NULL,
    [microservice_deleted] bit NULL,
    [microservice_core_connection] bit NULL,
    [delete_at] datetime2 NULL,
    [create_at] datetime2 NULL,
    [modify_at] datetime2 NULL,
    CONSTRAINT [PK_microservice_registers] PRIMARY KEY ([microservice_id]),
    CONSTRAINT [FK_microservice_registers_microservices_clusters_microservice_cluster_id] FOREIGN KEY ([microservice_cluster_id]) REFERENCES [microservices_clusters] ([microservices_cluster_id]) ON DELETE NO ACTION
);

CREATE TABLE [activity_logs] (
    [activity_log_id] uniqueidentifier NOT NULL,
    [event_type_id] bigint NULL,
    [activity_log_entity_name] nvarchar(255) NULL,
    [activity_log_entity_id] uniqueidentifier NULL,
    [activity_log_description] nvarchar(2000) NULL,
    [user_id] uniqueidentifier NULL,
    [create_at] datetime2 NULL,
    [modify_at] datetime2 NULL,
    CONSTRAINT [PK_activity_logs] PRIMARY KEY ([activity_log_id]),
    CONSTRAINT [FK_activity_logs_event_types_event_type_id] FOREIGN KEY ([event_type_id]) REFERENCES [event_types] ([event_type_id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_activity_logs_users_user_id] FOREIGN KEY ([user_id]) REFERENCES [users] ([user_id]) ON DELETE NO ACTION
);

CREATE TABLE [microservice_core_connector] (
    [microservice_core_connector_id] bigint NOT NULL IDENTITY,
    [core_connector_credential_id] bigint NULL,
    [microservice_id] bigint NULL,
    [create_at] datetime2 NULL,
    [modify_at] datetime2 NULL,
    CONSTRAINT [PK_microservice_core_connector] PRIMARY KEY ([microservice_core_connector_id]),
    CONSTRAINT [FK_microservice_core_connector_core_connector_credentials_core_connector_credential_id] FOREIGN KEY ([core_connector_credential_id]) REFERENCES [core_connector_credentials] ([core_connector_credential_id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_microservice_core_connector_microservice_registers_microservice_id] FOREIGN KEY ([microservice_id]) REFERENCES [microservice_registers] ([microservice_id]) ON DELETE CASCADE
);

CREATE TABLE [microservice_methods] (
    [microservice_method_id] bigint NOT NULL IDENTITY,
    [microservice_id] bigint NOT NULL,
    [microservice_method_delete] bit NULL,
    [microservice_method_name] nvarchar(255) NULL,
    [microservice_method_url] nvarchar(500) NULL,
    [create_at] datetime2 NULL,
    [modify_at] datetime2 NULL,
    CONSTRAINT [PK_microservice_methods] PRIMARY KEY ([microservice_method_id]),
    CONSTRAINT [FK_microservice_methods_microservice_registers_microservice_id] FOREIGN KEY ([microservice_id]) REFERENCES [microservice_registers] ([microservice_id]) ON DELETE CASCADE
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'core_connector_credential_id', N'core_connector_credential_key', N'core_connector_credential_pass', N'core_connector_credential_user', N'create_at', N'modify_at') AND [object_id] = OBJECT_ID(N'[core_connector_credentials]'))
    SET IDENTITY_INSERT [core_connector_credentials] ON;
INSERT INTO [core_connector_credentials] ([core_connector_credential_id], [core_connector_credential_key], [core_connector_credential_pass], [core_connector_credential_user], [create_at], [modify_at])
VALUES (CAST(1 AS bigint), N'api_key_001', N'encrypted_pass_001', N'auth_service_user', '2025-01-01T10:00:00.0000000Z', '2025-01-01T10:00:00.0000000Z'),
(CAST(2 AS bigint), N'api_key_002', N'encrypted_pass_002', N'product_service_user', '2025-01-01T10:00:00.0000000Z', '2025-01-01T10:00:00.0000000Z');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'core_connector_credential_id', N'core_connector_credential_key', N'core_connector_credential_pass', N'core_connector_credential_user', N'create_at', N'modify_at') AND [object_id] = OBJECT_ID(N'[core_connector_credentials]'))
    SET IDENTITY_INSERT [core_connector_credentials] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'event_type_id', N'create_at', N'event_type_description', N'modify_at') AND [object_id] = OBJECT_ID(N'[event_types]'))
    SET IDENTITY_INSERT [event_types] ON;
INSERT INTO [event_types] ([event_type_id], [create_at], [event_type_description], [modify_at])
VALUES (CAST(1 AS bigint), '2025-01-01T10:00:00.0000000Z', N'Microservice Registration', '2025-01-01T10:00:00.0000000Z'),
(CAST(2 AS bigint), '2025-01-01T10:00:00.0000000Z', N'Configuration Change', '2025-01-01T10:00:00.0000000Z');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'event_type_id', N'create_at', N'event_type_description', N'modify_at') AND [object_id] = OBJECT_ID(N'[event_types]'))
    SET IDENTITY_INSERT [event_types] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'microservices_cluster_id', N'create_at', N'delete_at', N'microservices_cluster_active', N'microservices_cluster_deleted', N'microservices_cluster_name', N'microservices_cluster_server_ip', N'microservices_cluster_server_name', N'modify_at') AND [object_id] = OBJECT_ID(N'[microservices_clusters]'))
    SET IDENTITY_INSERT [microservices_clusters] ON;
INSERT INTO [microservices_clusters] ([microservices_cluster_id], [create_at], [delete_at], [microservices_cluster_active], [microservices_cluster_deleted], [microservices_cluster_name], [microservices_cluster_server_ip], [microservices_cluster_server_name], [modify_at])
VALUES (CAST(1 AS bigint), '2025-01-01T10:00:00.0000000Z', NULL, CAST(1 AS bit), CAST(0 AS bit), N'Production Cluster', N'10.0.1.100', N'prod-cluster-01', '2025-01-01T10:00:00.0000000Z'),
(CAST(2 AS bigint), '2025-01-01T10:00:00.0000000Z', NULL, CAST(1 AS bit), CAST(0 AS bit), N'Development Cluster', N'10.0.2.100', N'dev-cluster-01', '2025-01-01T10:00:00.0000000Z');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'microservices_cluster_id', N'create_at', N'delete_at', N'microservices_cluster_active', N'microservices_cluster_deleted', N'microservices_cluster_name', N'microservices_cluster_server_ip', N'microservices_cluster_server_name', N'modify_at') AND [object_id] = OBJECT_ID(N'[microservices_clusters]'))
    SET IDENTITY_INSERT [microservices_clusters] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'user_id', N'create_at', N'email_confirmed', N'last_login', N'modify_at', N'password_changed_at', N'password_hash', N'refresh_token', N'refresh_token_expiry_time', N'user_active', N'user_email', N'user_name', N'user_peoplesoft') AND [object_id] = OBJECT_ID(N'[users]'))
    SET IDENTITY_INSERT [users] ON;
INSERT INTO [users] ([user_id], [create_at], [email_confirmed], [last_login], [modify_at], [password_changed_at], [password_hash], [refresh_token], [refresh_token_expiry_time], [user_active], [user_email], [user_name], [user_peoplesoft])
VALUES ('00000000-0000-0000-0000-000000000001', '2025-01-01T10:00:00.0000000Z', CAST(1 AS bit), NULL, '2025-01-01T10:00:00.0000000Z', NULL, N'$2a$11$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy', NULL, NULL, CAST(1 AS bit), N'admin@fastserver.com', N'Admin User', N'PS001'),
('00000000-0000-0000-0000-000000000002', '2025-01-01T10:00:00.0000000Z', CAST(1 AS bit), NULL, '2025-01-01T10:00:00.0000000Z', NULL, N'$2a$11$VRjNO0ZRwK7x1Z.XfJcKAOKs7ggzwhPB3QVpLp2PF3cxyMq7R5rHu', NULL, NULL, CAST(1 AS bit), N'developer@fastserver.com', N'Developer User', N'PS002');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'user_id', N'create_at', N'email_confirmed', N'last_login', N'modify_at', N'password_changed_at', N'password_hash', N'refresh_token', N'refresh_token_expiry_time', N'user_active', N'user_email', N'user_name', N'user_peoplesoft') AND [object_id] = OBJECT_ID(N'[users]'))
    SET IDENTITY_INSERT [users] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'activity_log_id', N'activity_log_description', N'activity_log_entity_id', N'activity_log_entity_name', N'create_at', N'event_type_id', N'modify_at', N'user_id') AND [object_id] = OBJECT_ID(N'[activity_logs]'))
    SET IDENTITY_INSERT [activity_logs] ON;
INSERT INTO [activity_logs] ([activity_log_id], [activity_log_description], [activity_log_entity_id], [activity_log_entity_name], [create_at], [event_type_id], [modify_at], [user_id])
VALUES ('10000000-0000-0000-0000-000000000001', N'AuthService registered successfully', '20000000-0000-0000-0000-000000000001', N'MicroserviceRegister', '2025-01-01T10:00:00.0000000Z', CAST(1 AS bigint), '2025-01-01T10:00:00.0000000Z', '00000000-0000-0000-0000-000000000001'),
('10000000-0000-0000-0000-000000000002', N'ProductService configuration updated', '20000000-0000-0000-0000-000000000002', N'MicroserviceRegister', '2025-01-01T10:05:00.0000000Z', CAST(2 AS bigint), '2025-01-01T10:05:00.0000000Z', '00000000-0000-0000-0000-000000000002');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'activity_log_id', N'activity_log_description', N'activity_log_entity_id', N'activity_log_entity_name', N'create_at', N'event_type_id', N'modify_at', N'user_id') AND [object_id] = OBJECT_ID(N'[activity_logs]'))
    SET IDENTITY_INSERT [activity_logs] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'microservice_id', N'create_at', N'delete_at', N'microservice_active', N'microservice_cluster_id', N'microservice_core_connection', N'microservice_deleted', N'microservice_name', N'modify_at') AND [object_id] = OBJECT_ID(N'[microservice_registers]'))
    SET IDENTITY_INSERT [microservice_registers] ON;
INSERT INTO [microservice_registers] ([microservice_id], [create_at], [delete_at], [microservice_active], [microservice_cluster_id], [microservice_core_connection], [microservice_deleted], [microservice_name], [modify_at])
VALUES (CAST(1 AS bigint), '2025-01-01T10:00:00.0000000Z', NULL, CAST(1 AS bit), CAST(1 AS bigint), CAST(1 AS bit), CAST(0 AS bit), N'AuthService', '2025-01-01T10:00:00.0000000Z'),
(CAST(2 AS bigint), '2025-01-01T10:00:00.0000000Z', NULL, CAST(1 AS bit), CAST(1 AS bigint), CAST(1 AS bit), CAST(0 AS bit), N'ProductService', '2025-01-01T10:00:00.0000000Z');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'microservice_id', N'create_at', N'delete_at', N'microservice_active', N'microservice_cluster_id', N'microservice_core_connection', N'microservice_deleted', N'microservice_name', N'modify_at') AND [object_id] = OBJECT_ID(N'[microservice_registers]'))
    SET IDENTITY_INSERT [microservice_registers] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'microservice_core_connector_id', N'core_connector_credential_id', N'create_at', N'microservice_id', N'modify_at') AND [object_id] = OBJECT_ID(N'[microservice_core_connector]'))
    SET IDENTITY_INSERT [microservice_core_connector] ON;
INSERT INTO [microservice_core_connector] ([microservice_core_connector_id], [core_connector_credential_id], [create_at], [microservice_id], [modify_at])
VALUES (CAST(1 AS bigint), CAST(1 AS bigint), '2025-01-01T10:00:00.0000000Z', CAST(1 AS bigint), '2025-01-01T10:00:00.0000000Z'),
(CAST(2 AS bigint), CAST(2 AS bigint), '2025-01-01T10:00:00.0000000Z', CAST(2 AS bigint), '2025-01-01T10:00:00.0000000Z');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'microservice_core_connector_id', N'core_connector_credential_id', N'create_at', N'microservice_id', N'modify_at') AND [object_id] = OBJECT_ID(N'[microservice_core_connector]'))
    SET IDENTITY_INSERT [microservice_core_connector] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'microservice_method_id', N'create_at', N'microservice_id', N'microservice_method_delete', N'microservice_method_name', N'microservice_method_url', N'modify_at') AND [object_id] = OBJECT_ID(N'[microservice_methods]'))
    SET IDENTITY_INSERT [microservice_methods] ON;
INSERT INTO [microservice_methods] ([microservice_method_id], [create_at], [microservice_id], [microservice_method_delete], [microservice_method_name], [microservice_method_url], [modify_at])
VALUES (CAST(1 AS bigint), '2025-01-01T10:00:00.0000000Z', CAST(1 AS bigint), CAST(0 AS bit), N'AuthenticateUser', N'/api/users/authenticate', '2025-01-01T10:00:00.0000000Z'),
(CAST(2 AS bigint), '2025-01-01T10:00:00.0000000Z', CAST(2 AS bigint), CAST(0 AS bit), N'SearchProducts', N'/api/products/search', '2025-01-01T10:00:00.0000000Z');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'microservice_method_id', N'create_at', N'microservice_id', N'microservice_method_delete', N'microservice_method_name', N'microservice_method_url', N'modify_at') AND [object_id] = OBJECT_ID(N'[microservice_methods]'))
    SET IDENTITY_INSERT [microservice_methods] OFF;

CREATE INDEX [IX_activity_logs_activity_log_entity_name] ON [activity_logs] ([activity_log_entity_name]);

CREATE INDEX [IX_activity_logs_create_at] ON [activity_logs] ([create_at]);

CREATE INDEX [IX_activity_logs_event_type_id] ON [activity_logs] ([event_type_id]);

CREATE INDEX [IX_activity_logs_user_id] ON [activity_logs] ([user_id]);

CREATE INDEX [IX_ActivityLog_UserId_CreateAt_Desc] ON [activity_logs] ([user_id], [create_at] DESC);

CREATE UNIQUE INDEX [UX_CoreConnectorCredential_User] ON [core_connector_credentials] ([core_connector_credential_user]) WHERE [core_connector_credential_user] IS NOT NULL;

CREATE INDEX [IX_event_types_event_type_description] ON [event_types] ([event_type_description]);

CREATE INDEX [IX_microservice_core_connector_core_connector_credential_id] ON [microservice_core_connector] ([core_connector_credential_id]);

CREATE INDEX [IX_microservice_core_connector_microservice_id] ON [microservice_core_connector] ([microservice_id]);

CREATE INDEX [IX_microservice_methods_microservice_id] ON [microservice_methods] ([microservice_id]);

CREATE INDEX [IX_microservice_methods_microservice_method_name] ON [microservice_methods] ([microservice_method_name]);

CREATE INDEX [IX_microservice_registers_microservice_cluster_id] ON [microservice_registers] ([microservice_cluster_id]);

CREATE INDEX [IX_microservice_registers_microservice_name] ON [microservice_registers] ([microservice_name]);

CREATE INDEX [IX_MicroserviceRegister_ClusterId_Name] ON [microservice_registers] ([microservice_cluster_id], [microservice_name]);

CREATE INDEX [IX_microservices_clusters_microservices_cluster_name] ON [microservices_clusters] ([microservices_cluster_name]);

CREATE UNIQUE INDEX [IX_users_user_email] ON [users] ([user_email]) WHERE [user_email] IS NOT NULL;

CREATE INDEX [IX_users_user_peoplesoft] ON [users] ([user_peoplesoft]);

INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260210013239_InitialCreate', N'10.0.2');

COMMIT;
GO

BEGIN TRANSACTION;
INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260210014016_SqlServerInitialCreate', N'10.0.2');

COMMIT;
GO

