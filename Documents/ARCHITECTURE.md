# FlexCore Architecture Documentation

## Overview
Modular .NET framework per applicazioni enterprise con:
- ✅ Cache multistrato (Memory/Redis)  
- ✅ Database-agnostic (SQL/NoSQL)  
- ✅ Logging estendibile  
- ✅ Sicurezza JWT/OAuth  
- ✅ Supporto ORM ibrido (EF Core/Dapper/ADO)  

---

## Core Modules

### 1. Caching
| Componente              | Descrizione                                  | File                     |
|-------------------------|----------------------------------------------|--------------------------|
| `CacheProviderFactory`  | Factory per provider di cache                | [CacheProviderFactory.cs] |
| `BaseCacheManager`      | Classe base per operazioni CRUD sulla cache  | [BaseCacheManager.cs]     |
| `RedisCacheProvider`    | Implementazione Redis                        | [RedisCacheProvider.cs]   |
| **Flusso**              | `Get → [Memory → Redis] → Fallback DB`       |                          |

### 2. Database
| Componente              | Descrizione                                  | File                     |
|-------------------------|----------------------------------------------|--------------------------|
| `IUnitOfWork`           | Gestione transazioni ACID                   | [IUnitOfWork.cs]         |
| `DatabaseProviderFactory`| Factory per connessioni DB                  | [DatabaseProviderFactory.cs] |
| `ApplicationDbContext`  | Contesto EF Core con transazioni             | [ApplicationDbContext.cs] |

### 3. Logging
| Componente              | Descrizione                                  | File                     |
|-------------------------|----------------------------------------------|--------------------------|
| `ILoggingProvider`      | Interfaccia per logger (Console/File/Cloud)  | [ILoggingProvider.cs]    |
| `SerilogLogger`         | Implementazione Serilog                      | [SerilogLogger.cs]       |
| **Pipeline**            | `Log → [Filtri] → [Formattazione] → Sink`    |                          |

### 4. Security
| Componente              | Descrizione                                  | File                     |
|-------------------------|----------------------------------------------|--------------------------|
| `JwtTokenService`       | Generazione/validazione token JWT            | [JwtTokenService.cs]     |
| `GoogleTokenValidator`  | Integrazione OAuth con Google                | [GoogleTokenValidator.cs] |
| `PolicyService`         | Autorizzazione basata su ruoli               | [PolicyService.cs]       |

### 5. ORM
| Componente              | Descrizione                                  | File                     |
|-------------------------|----------------------------------------------|--------------------------|
| `OrmProviderFactory`    | Seleziona dinamicamente EF/Dapper/ADO        | [OrmProviderFactory.cs]  |
| `EFCoreOrmProvider`     | Implementazione EF Core                      | [EFCoreOrmProvider.cs]   |
| **Pattern**             | Unit of Work + Repository                    |                          |

### 6. Configuration
| Componente              | Descrizione                                  | File                     |
|-------------------------|----------------------------------------------|--------------------------|
| `ConfigurationAdapter`  | Binding delle impostazioni da appsettings.json | [ConfigurationAdapter.cs] |
| `AppSettings`           | Modello per configurazioni globali           | [AppSettings.cs]         |

### 7. Infrastructure
| Componente              | Descrizione                                  | File                     |
|-------------------------|----------------------------------------------|--------------------------|
| `EventBus`              | Publish/Subscribe eventi di dominio          | [EventBus.cs]            |
| `CommandBase`           | Implementazione base del pattern CQRS        | [CommandBase.cs]         |

---

## Diagramma Architetturale
```ascii
                   +-------------------+
                   |   API/Client      |
                   +-------------------+
                            ↓
                   +-------------------+
                   |   Security Layer  |
                   | (JWT/OAuth/Policies) 
                   +-------------------+
                            ↓
                   +-------------------+
                   |   Caching Layer   |
                   | (Memory → Redis)  |
                   +-------------------+
                            ↓
                   +-------------------+
                   |   Business Logic  |
                   +-------------------+
                            ↓
                   +-------------------+
                   |   Data Layer      |
                   | (ORM → Database)  |
                   +-------------------+
                            ↓
                   +-------------------+
                   |   Logging/Monitoring 
                   | (Serilog → Cloud) |
                   +-------------------+