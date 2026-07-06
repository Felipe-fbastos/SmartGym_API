# SmartGym API

API REST desenvolvida em **.NET 8 (ASP.NET Core)** para gerenciamento de uma academia (gym), contemplando o controle de **membros (clientes)**, **funcionários/instrutores**, **turmas (aulas)**, **matrículas em turmas** e o vínculo entre membros e treinadores.

## Funcionalidades

- **Autenticação via JWT** para membros e funcionários
- **Cadastro e login** de membros (clientes da academia)
- **Cadastro e login** de funcionários (instrutores/administradores)
- **Regras de negócio nas inscrições**, como idade mínima:
  - Membros: acima de 6 anos
  - Funcionários: acima de 18 anos
- **Soft delete** (exclusão lógica) de membros e funcionários
- **Gestão de papéis (roles)**, com nomes únicos
- **Gestão de turmas** (`GymClass`), com validações de data/duração e vínculo a um instrutor
- **Matrícula de membros em turmas**, respeitando o limite de vagas (`Capacity`)
- **Vínculo entre membros e treinadores** (`MemberTrainer`), com ativação/desativação (soft delete) e regra para evitar vínculo duplicado
- **Tratamento centralizado de exceções** via middleware
- Documentação automática com **Swagger**

> ⚠️ Os controllers ainda não possuem atributos `[Authorize]`/`[AllowAnonymous]` configurados — a autenticação JWT está pronta na pipeline, mas a proteção por rota/papel ainda precisa ser aplicada.

## Tecnologias

- [.NET 8](https://dotnet.microsoft.com/) / ASP.NET Core Web API
- Entity Framework Core 8 (SQL Server)
- Autenticação JWT (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- Mapster (mapeamento objeto-objeto / DTOs)
- ASP.NET Core Identity `PasswordHasher` (hash de senhas)
- Swashbuckle (Swagger/OpenAPI)

## Estrutura do projeto

```
SmartGym_API/
├── Controllers/          # EmployeeController, MemberController, GymClassController,
│                          # MemberTrainerController, RoleController
├── Data/                  # AppDbContext (EF Core)
├── DTO/
│   ├── Employee/          # DTOs de funcionário (create, update, login, response)
│   ├── Member/            # DTOs de membro (create, update, login, response)
│   ├── GymClass/          # DTOs de turma e matrícula
│   ├── MemberTrainer/      # DTOs do vínculo membro-treinador
│   └── Roles/              # DTOs de papel (role)
├── Exceptions/             # Exceções customizadas (NotFound, Conflict, BadRequest, etc.)
├── Mapper/                  # Configuração do Mapster
├── Middleware/               # Middleware global de tratamento de exceções
├── Migrations/                # Migrations do Entity Framework
├── Models/                     # Entidades: Employee, Member, GymClass, MemberTrainer, Roles
├── Service/                     # Regras de negócio de cada domínio + TokenService
└── Program.cs                    # Configuração da aplicação
```

## Modelo de dados (principais entidades)

| Entidade | Descrição |
|---|---|
| `Member` | Cliente/membro da academia |
| `Employee` | Funcionário/instrutor |
| `Roles` | Papel do usuário (relacionado a `Member` e `Employee`) |
| `GymClass` | Turma/aula ministrada por um `Employee`, com capacidade e nº de matrículas |
| `MemberTrainer` | Relação entre um `Member` e o `Employee` responsável por seu treino |

## Endpoints principais

### Membros (`/api/Member`)

| Método | Rota | Descrição |
|---|---|---|
| POST | `/api/Member/signup` | Cadastra um novo membro |
| POST | `/api/Member/login` | Autentica e retorna um token JWT |
| GET | `/api/Member/me` | Retorna os dados do membro autenticado |
| GET | `/api/Member/{id}` | Busca um membro por id |
| GET | `/api/Member` | Lista todos os membros |
| PUT | `/api/Member/me` | Atualiza os dados do membro autenticado |
| PUT | `/api/Member/{id}` | Atualiza um membro por id |
| DELETE | `/api/Member/{id}` | Exclusão lógica de um membro |

### Funcionários (`/api/Employee`)

| Método | Rota | Descrição |
|---|---|---|
| POST | `/api/Employee` | Cadastra um novo funcionário |
| POST | `/api/Employee` | Autentica e retorna um token JWT (login) |
| GET | `/api/Employee/me` | Retorna os dados do funcionário autenticado |
| GET | `/api/Employee/{id}` | Busca um funcionário por id |
| GET | `/api/Employee` | Lista todos os funcionários |
| PUT | `/api/Employee` | Atualiza os dados do funcionário autenticado |
| PUT | `/api/Employee/{id}` | Atualiza um funcionário por id |
| DELETE | `/api/Employee/{id}` | Exclusão lógica de um funcionário |

### Turmas (`/api/GymClass`)

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/GymClass/{id}` | Busca uma turma por id |
| GET | `/api/GymClass` | Lista todas as turmas |
| POST | `/api/GymClass` | Cria uma nova turma |
| PUT | `/api/GymClass/{id}` | Atualiza uma turma |
| POST | `/api/GymClass` | Matricula o membro autenticado em uma turma (mesma rota de criação, ver observação acima) |

### Vínculo membro-treinador (`/api/MemberTrainer`)

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/MemberTrainer/{id}` | Busca um vínculo ativo por id |
| GET | `/api/MemberTrainer` | Lista todos os vínculos ativos |
| POST | `/api/MemberTrainer` | Cria um novo vínculo entre membro e treinador |
| DELETE | `/api/MemberTrainer/{id}` | Desativa (soft delete) um vínculo |

### Papéis (`/api/Role`)

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/Role/{id}` | Busca um papel por id |
| GET | `/api/Role` | Lista todos os papéis |
| POST | `/api/Role` | Cria um novo papel (nome único) |

## Regras de negócio implementadas nos services

- **Cadastro de membro:** valida e-mail único e idade mínima de 6 anos
- **Cadastro de funcionário:** valida e-mail único e idade mínima de 18 anos
- **Login:** valida credenciais com hash de senha e gera token JWT contendo `Id`, `Email` e `Role`
- **Exclusão de membros/funcionários:** implementada como *soft delete* (`IsDelete = true`), preservando o histórico
- **Criação de turma:** valida existência do instrutor, que a data de término seja posterior à de início, que o início não seja no passado e que a duração não ultrapasse 24 horas
- **Matrícula em turma:** impede matrícula além da capacidade (`Capacity`) da turma
- **Vínculo membro-treinador:** valida existência de membro e treinador e impede duplicar um vínculo já ativo; desativação registra a data de encerramento (`DissolvedAt`)
- **Papéis:** impede a criação de papéis com nome duplicado

## Como executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (local ou remoto)

### Configuração

1. Clone o repositório:
   ```bash
   git clone https://github.com/Felipe-fbastos/SmartGym_API.git
   cd SmartGym_API
   ```

2. Configure a connection string e as chaves do JWT em `appsettings.json` (ou, de preferência, via **User Secrets**):
   ```json
   {
     "ConnectionStrings": {
       "Somee": "SUA_CONNECTION_STRING_AQUI"
     },
     "Jwt": {
       "Issuer": "MyApi",
       "Audience": "MyApiUsers",
       "Key": "SUA_CHAVE_SECRETA_AQUI"
     }
   }
   ```

3. Aplique as migrations para criar o banco de dados:
   ```bash
   dotnet ef database update
   ```

4. Rode a aplicação:
   ```bash
   dotnet run
   ```

5. Acesse o Swagger (em ambiente de desenvolvimento) para explorar a API:
   ```
   https://localhost:{porta}/swagger
   ```

## Próximos passos sugeridos

- Adicionar `[Authorize]` (e restrições por role, quando aplicável) nos controllers
- Resolver a sobreposição de rota entre criação e login de `Employee`, e entre criação e matrícula de `GymClass`
- Adicionar testes automatizados

## Autor

Desenvolvido por [Felipe-fbastos](https://github.com/Felipe-fbastos).
