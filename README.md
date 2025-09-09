# Gestão de Pedidos

Este é um sistema completo para gestão de pedidos, construído com uma arquitetura de microsserviços desacoplada, utilizando .NET para o backend, Angular para o frontend e Docker para orquestração do ambiente.

## Visão Geral do Sistema

O sistema permite o cadastro de clientes e o enfileiramento de novos pedidos. A comunicação entre a API principal e um serviço de consumidor é feita através de uma fila (RabbitMQ), garantindo que o processamento de pedidos ocorra de forma assíncrona e resiliente.

O fluxo principal é:
1.  O frontend (Angular) interage com a API REST (.NET).
2.  Um novo pedido é criado através do endpoint `POST /api/Pedidos`.
3.  A API não processa o pedido diretamente; em vez disso, publica uma mensagem em uma fila do RabbitMQ.
4.  O serviço `GestaoPedidos.Consumer` escuta a fila, consome a mensagem e efetiva o processamento do pedido, salvando-o no banco de dados.
5.  O frontend pode consultar o status e os detalhes dos pedidos e clientes através de outros endpoints da API.

## Frameworks e Bibliotecas Principais

Esta seção detalha os principais frameworks e bibliotecas que compõem a arquitetura do sistema.

### Backend (.NET)

-   **ASP.NET Core**: É o principal framework para a construção da API REST. Ele gerencia o roteamento das requisições HTTP, o ciclo de vida dos controllers, a injeção de dependências e o servidor web (Kestrel).
-   **Entity Framework Core**: Atua como o ORM (Object-Relational Mapper), fazendo a ponte entre o código C# (nossas classes de `Entidades`) e o banco de dados relacional (PostgreSQL). Ele simplifica todas as operações de banco de dados, desde consultas complexas até a criação e atualização de registros.
-   **MediatR**: Biblioteca que implementa o padrão Mediator. É a peça central da nossa implementação de CQRS, desacoplando quem envia uma requisição (o `Controller`) de quem a processa (os `Handlers`). Isso torna o código mais limpo, organizado e fácil de testar.
-   **Npgsql**: É o provedor de dados .NET para PostgreSQL. Permite que o Entity Framework Core se comunique de forma eficiente com o banco de dados PostgreSQL.
-   **RabbitMQ.Client**: Biblioteca oficial do .NET para interagir com o RabbitMQ. É utilizada para publicar mensagens na fila (pela API) e para consumir essas mensagens (pelo serviço `Consumer`).

### Frontend (Angular)

-   **Angular**: Framework SPA (Single-Page Application) utilizado para construir a interface do usuário. Ele é responsável por renderizar os componentes, gerenciar a navegação entre as telas (roteamento) e controlar o estado da aplicação no lado do cliente.
-   **Angular Material**: Biblioteca de componentes de UI para Angular. Fornece um conjunto de componentes pré-construídos e estilizados (como tabelas, cards, botões, inputs e painéis expansíveis) que garantem uma experiência de usuário moderna e consistente.
-   **RxJS (Reactive Extensions for JavaScript)**: É uma biblioteca fundamental no ecossistema Angular para programação reativa. Ela utiliza `Observables` para gerenciar operações assíncronas, como as chamadas HTTP para a nossa API, facilitando a manipulação de fluxos de dados ao longo do tempo.

## Arquitetura e Padrões de Design (Design Patterns)

O projeto foi estruturado utilizando padrões de design modernos para garantir escalabilidade, manutenibilidade e um baixo acoplamento entre os componentes.

### 1. Injeção de Dependência (Dependency Injection)
-   **Descrição**: É o padrão central da arquitetura .NET. As dependências de uma classe (como serviços ou repositórios) são fornecidas por uma fonte externa (o contêiner de DI) em vez de serem criadas internamente.
-   **Implementação**: O contêiner de DI do ASP.NET Core é configurado em `Program.cs` para registrar serviços com diferentes ciclos de vida:
    -   **Scoped**: Uma instância por requisição (ex: `AppDbContext`, `IClienteRepository`). Ideal para compartilhar o mesmo contexto em uma única transação HTTP.
    -   **Singleton**: Uma única instância para toda a aplicação (ex: serviços de infraestrutura como a conexão com RabbitMQ).
    -   **Transient**: Uma nova instância é criada a cada vez que o serviço é solicitado.
-   **Onde ver**: Na configuração de serviços em `Program.cs` e nos construtores da maioria das classes de serviço e controllers.

### 2. CQRS (Command Query Responsibility Segregation)
-   **Descrição**: Separa as operações de escrita (Commands) das de leitura (Queries), permitindo otimizar cada caminho de forma independente.
-   **Implementação**: Utilizamos a biblioteca **MediatR** para orquestrar esse padrão.
    -   **Commands**: `CadastrarClienteCommand`, `EnfileirarPedidoCommand`.
    -   **Queries**: `ObterPedidosPorClienteQuery`.
-   **Onde ver**: `GestaoPedidos.Application/Clientes/Commands`, `GestaoPedidos.Application/Pedidos/Queries`.

### 3. Mediator Pattern
-   **Descrição**: Centraliza a comunicação entre objetos, evitando que eles se refiram uns aos outros explicitamente. O MediatR atua como o mediador.
-   **Implementação**: Em vez de um controller injetar múltiplos serviços, ele apenas injeta o `IMediator` e envia `Commands` ou `Queries`. O mediador se encarrega de encontrar o `Handler` apropriado.
-   **Onde ver**: `ClientesController` e `PedidosController` utilizando `_mediator.Send(...)`.

### 4. Repository e Unit of Work Patterns
-   **Descrição**: O **Repository** abstrai a lógica de acesso a dados, enquanto o **Unit of Work** agrupa múltiplas operações em uma única transação.
-   **Implementação**:
    -   **Repository**: Interfaces como `IClienteRepository` definem o contrato, e as classes como `ClienteRepository` o implementam usando o `AppDbContext`.
    -   **Unit of Work**: O próprio `AppDbContext` do EF Core funciona como uma implementação deste padrão. A chamada a `_context.SaveChangesAsync()` consolida todas as alterações pendentes em uma única transação.
-   **Onde ver**: `GestaoPedidos.Application/Interfaces/Repositories`, `GestaoPedidos.Infrastructure/Data/Repositories`.

### 5. Producer/Consumer com Fila de Mensagens
-   **Descrição**: Desacopla operações demoradas ou críticas, onde a API (Producer) publica uma mensagem em uma fila e um serviço de background (Consumer) a processa de forma assíncrona.
-   **Implementação**: A API publica um pedido no RabbitMQ, e o `GestaoPedidos.Consumer` o processa.
-   **Onde ver**: `PedidosController` e `RabbitMqConsumer.cs`.

### 6. Options Pattern
-   **Descrição**: Vincula seções de configuração (de `appsettings.json`) a classes C# fortemente tipadas, permitindo o acesso seguro e testável às configurações.
-   **Implementação**: `builder.Services.Configure<MySettings>(...)` em `Program.cs` e a injeção de `IOptions<MySettings>` nos serviços.

## Execução do Projeto

### Pré-requisitos
-   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
-   [Docker e Docker Compose](https://www.docker.com/products/docker-desktop)
-   [Node.js e Angular CLI](https://angular.io/cli) (Opcional, para desenvolvimento do frontend fora do Docker)

### Método 1: Docker Compose (Recomendado)

A forma mais simples de executar todo o ambiente (API, Consumer, Frontend, Postgres, RabbitMQ) é com o Docker Compose.

1.  Navegue até a raiz do projeto.
2.  Execute o comando:
    ```bash
    docker-compose up --build -d
    ```
3.  Aguarde todos os contêineres iniciarem. Os serviços estarão disponíveis nos seguintes endereços:
    -   **Frontend (Angular)**: `http://localhost:4200`
    -   **API (.NET)**: `http://localhost:8080`
    -   **RabbitMQ Management**: `http://localhost:15672` (login: guest/guest)

### Método 2: Execução Local (Para Desenvolvimento)

Você pode executar cada parte do projeto individualmente. Lembre-se que o Postgres e o RabbitMQ precisam estar em execução (recomenda-se iniciá-los via Docker Compose).

-   **API**:
    ```bash
    cd GestaoPedidos.API
    dotnet run
    ```
-   **Consumer**:
    ```bash
    cd GestaoPedidos.Consumer
    dotnet run
    ```
-   **Frontend**:
    ```bash
    cd GestaoPedidos.FrontEnd/gestao-projetos
    npm install
    ng serve
    ```

## Banco de Dados (PostgreSQL)

### Migrations do Entity Framework Core

O projeto utiliza EF Core para gerenciar o schema do banco de dados.

-   **Para criar uma nova migração** (após alterar uma entidade do domínio):
    ```bash
    dotnet ef migrations add NomeDaSuaMigration --project GestaoPedidos.Infrastructure --startup-project GestaoPedidos.API
    ```

-   **Para aplicar as migrações ao banco de dados**:
    ```bash
    dotnet ef database update --project GestaoPedidos.Infrastructure --startup-project GestaoPedidos.API
    ```

### Geração de Scripts SQL

Se você precisar gerar um script SQL a partir das migrações para execução manual, utilize o comando abaixo.

-   **Para gerar um script com todas as migrações**:
    ```bash
    dotnet ef migrations script --project GestaoPedidos.Infrastructure --startup-project GestaoPedidos.API -o ./db-script.sql
    ```
    Isso criará um arquivo `db-script.sql` na raiz do projeto.

## Exemplos de Uso da API (com cURL)

Aqui estão alguns exemplos de como interagir com a API.

### 1. Cadastrar um novo Cliente
```bash
curl -X POST http://localhost:8080/api/Clientes \
-H "Content-Type: application/json" \
-d '{
  "nome": "Novo Cliente Exemplo"
}'
```

### 2. Enfileirar um novo Pedido
Este comando envia um pedido para a fila do RabbitMQ para ser processado pelo consumidor.
```bash
curl -X POST http://localhost:8080/api/Pedidos \
-H "Content-Type: application/json" \
-d '{
  "clienteId": 1,
  "itens": [
    {
      "produto": "Produto A",
      "quantidade": 2,
      "precoUnitario": 150.75
    },
    {
      "produto": "Produto B",
      "quantidade": 1,
      "precoUnitario": 500.00
    }
  ]
}'
```

### 3. Consultar todos os pedidos detalhados
```bash
curl -X GET http://localhost:8080/api/Pedidos
```

### 4. Consultar pedidos de um cliente específico
```bash
# Substitua {clienteId} pelo ID do cliente
curl -X GET http://localhost:8080/api/Pedidos/cliente/{clienteId}
```
