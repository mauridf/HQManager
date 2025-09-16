# HQManager - Sistema de Gestão de HQs

## 📋 Resumo Técnico do Projeto

O **HQManager** é um sistema completo para gerenciamento de coleções de Histórias em Quadrinhos, desenvolvido com arquitetura moderna seguindo princípios SOLID e Clean Architecture.

### 🏗️ Arquitetura e Tecnologias

- **Backend**: .NET 9 + C# 11
- **Banco de Dados**: MongoDB (NoSQL)
- **Autenticação**: JWT (JSON Web Tokens)
- **Hash de Senhas**: BCrypt.Net-Next
- **Documentação**: Swagger/OpenAPI
- **IDE**: Visual Studio 2022

### 📦 Estrutura da Solução

```
HQManager/
├── HQManager.API/           # Camada de apresentação (Web API)
├── HQManager.Application/   # Casos de uso e DTOs
├── HQManager.Domain/        # Entidades, interfaces e enums
├── HQManager.Infra.Data/    # Implementação do MongoDB
└── HQManager.CrossCutting/  # Serviços compartilhados
```

### 🗃️ Modelo de Dados (MongoDB Collections)

1. **Usuarios** - Sistema de autenticação
2. **Editoras** - Editoras de HQs
3. **Personagens** - Personagens com tipos específicos
4. **Equipes** - Grupos de personagens
5. **HQs** - Obras principais com relacionamentos
6. **Edicoes** - Números/capítulos individuais

### 🔐 Sistema de Autenticação

- Registro de usuários com validação de email único
- Login com JWT (120 minutos de validade)
- Hash de senhas com BCrypt
- Endpoints protegidos com `[Authorize]`

### 📊 Entidades e Funcionalidades

#### 👤 Usuário
- Cadastro e autenticação
- Controle de último login

#### 🏢 Editora
- CRUD completo
- Campos: Nome, ano, país, logo, site

#### 🦸 Personagem
- CRUD completo
- Tipo enumerado: Herói, Vilão, Anti-Herói, etc.
- Primeira aparição e descrição

#### 👥 Equipe
- CRUD completo
- Grupos de personagens

#### 📚 HQ (Entidade Principal)
- CRUD com validação de relacionamentos
- **Regra de negócio**: Deve ter pelo menos 1 personagem OU 1 equipe
- Tipos: Mensal, Minissérie, Evento, etc.
- Status: Em Andamento, Finalizado, etc.
- Sistema de tags e recomendações

#### 📖 Edição
- CRUD vinculado à HQ
- Controle de leitura (lida/não lida)
- Sistema de notas (0-10)
- Estatísticas de progresso

### 🚀 Como Executar

#### Pré-requisitos
- .NET 9 SDK
- MongoDB (local ou Atlas)
- Visual Studio 2022 (recomendado)

#### Configuração

1. **Clone o repositório**
```bash
git clone <url-do-repositorio>
cd HQManager
```

2. **Configure o MongoDB**
   - Instale o MongoDB local ou use MongoDB Atlas
   - Atualize a connection string no `appsettings.json`:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "HQManagerDB"
  }
}
```

3. **Configure o JWT** (em produção use variáveis de ambiente)
```json
{
  "JwtSettings": {
    "SecretKey": "SUA_CHAVE_SECRETA_MUITO_SEGURA_AQUI",
    "Issuer": "hqmanager-api",
    "Audience": "hqmanager-app",
    "ExpiresInMinutes": 120
  }
}
```

4. **Execute a aplicação**
```bash
dotnet restore
dotnet run --project HQManager.API
```

5. **Acesse a documentação**
   - Swagger UI: `https://localhost:7214/swagger`
   - Health Check: `https://localhost:7214/api/health`

### 🔧 Endpoints Principais

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/usuarios` | Registrar novo usuário |
| POST | `/api/auth/login` | Fazer login |
| GET | `/api/editoras` | Listar editoras |
| GET | `/api/personagens` | Listar personagens |
| GET | `/api/hqs` | Listar HQs |
| GET | `/api/edicoes/hq/{id}` | Listar edições de uma HQ |
| POST | `/api/hqs` | Criar nova HQ |

### 🛡️ Validações Implementadas

- **Validação de modelos** com DataAnnotations
- **Validação de relacionamentos** (IDs existentes)
- **Regras de negócio** específicas
- **Autenticação JWT** em endpoints protegidos
- **Validação de unicidade** (email, número de edição)

### 📈 Funcionalidades Avançadas

- **Busca por relacionamentos**: HQs por personagem, equipe, editora
- **Estatísticas de leitura**: Progresso e médias por HQ
- **Controle de edições lidas**: Marcar/desmarcar como lida
- **Sistema de recomendações**: HQs relacionadas

### 🔍 Monitoramento e Saúde

- **Health Check**: `/api/health` verifica conexão com MongoDB
- **Swagger**: Documentação interativa da API
- **Logs**: Configurado com diferentes níveis

### 📚 Links Úteis

- [Documentação .NET 9](https://learn.microsoft.com/dotnet/core/)
- [MongoDB .NET Driver](https://www.mongodb.com/docs/drivers/csharp/)
- [JWT em ASP.NET Core](https://learn.microsoft.com/aspnet/core/security/authentication/jwt)
- [Swagger/OpenAPI](https://swagger.io/)

### 🎯 Próximas Melhorias

1. **Upload de imagens** (Azure Blob Storage/AWS S3)
2. **Sistema de permissões** (Admin/Usuário)
3. **Busca avançada** e filtros
4. **Importação/exportação** de dados
5. **API GraphQL** alternativa
6. **Cache** com Redis
7. **Background services** para tarefas periódicas

### 👨‍💻 Desenvolvimento

Padrões adotados:
- **Clean Architecture**
- **Repository Pattern**
- **Dependency Injection**
- **Async/Await** em todas as operações de I/O
- **Commit semântico** (Conventional Commits)

---

**Status**: ✅ API completa e funcional  
**Versão**: 1.0.0  
**Última atualização**: Setembro 2025
