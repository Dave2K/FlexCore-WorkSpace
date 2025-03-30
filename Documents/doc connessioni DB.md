```markdown
# 🔌 Varianti di Connessione per Tutti i Provider

## 1. SQL Server
**Configurazione JSON:**
```json
"ConnectionStrings": {
  "SQLServer": "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;"
}

// Connessione Trusted
"SQLServer_Trusted": "Server=localhost;Database=FlexCore;Trusted_Connection=True;"
```

**Programmatico:**
```csharp
services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(
        Configuration.GetConnectionString("SQLServer"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
);
```

---

## 2. SQLite
**Configurazione JSON:**
```json
"ConnectionStrings": {
  "SQLite": "Data Source=./Data/FlexCore.db;Cache=Shared;"
}

// Modalità memoria
"SQLite_InMemory": "Data Source=:memory:;"
```

**Programmatico:**
```csharp
services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite(
        Configuration.GetConnectionString("SQLite"),
        sqliteOptions => sqliteOptions.CommandTimeout(30)
    )
);
```

---

## 3. MariaDB/MySQL
**Configurazione JSON:**
```json
"ConnectionStrings": {
  "MariaDB": "Server=localhost;Port=3306;Database=flexcore;User=root;Password=myPassword;"
}
```

**Programmatico:**
```csharp
services.AddDbContext<AppDbContext>(options => 
    options.UseMySql(
        Configuration.GetConnectionString("MariaDB"),
        new MySqlServerVersion(new Version(10, 5, 12)),
        mysqlOptions => mysqlOptions.EnableRetryOnFailure()
    )
);
```

---

## 4. Redis
**Configurazione JSON:**
```json
"ConnectionStrings": {
  "Redis": "flexcore-redis:6379,password=myRedisPassword,ssl=True,abortConnect=False"
}
```

**Programmatico:**
```csharp
services.AddStackExchangeRedisCache(options => {
    options.Configuration = Configuration.GetConnectionString("Redis");
    options.InstanceName = "FlexCoreCache";
});
```

---

## 5. Configurazione Multi-Provider (Dynamic)
```csharp
var provider = Configuration["DatabaseSettings:DefaultProvider"];

switch(provider) {
    case "SQLServer":
        services.AddSqlServer<AppDbContext>(
            Configuration.GetConnectionString("SQLServer"));
        break;
        
    case "SQLite":
        services.AddSqlite<AppDbContext>(
            Configuration.GetConnectionString("SQLite"));
        break;
        
    case "MariaDB":
        services.AddMySql<AppDbContext>(
            Configuration.GetConnectionString("MariaDB"));
        break;
}
```

---

## 📌 Note Essenziali:
1. **Sicurezza:**  
   ```csharp
   // Usare sempre Configuration Manager/Key Vault in produzione
   var connString = config.GetConnectionString("SQLServer") 
       + $";Password={config["DbPassword"]}"; // Da secrets
   ```

2. **Timeout:**  
   ```json
   "ConnectionStrings": {
     "SQLServer": "Server=...;Connect Timeout=30;"
   }
   ```

3. **Pooling:**  
   ```json
   "SQLServer": "Server=...;Max Pool Size=100;Min Pool Size=10;"
   ```

4. **SSL:**  
   ```json
   "MariaDB": "Server=...;SslMode=Required;"
   ```
```