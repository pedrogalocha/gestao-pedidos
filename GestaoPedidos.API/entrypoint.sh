#!/bin/bash
set -e

until pg_isready -h "postgres" -U "postgres" -q; do
  echo "Aguardando o banco de dados iniciar..."
  sleep 2
done

echo "Banco de dados pronto! Executando as migrações..."
dotnet ef database update --project GestaoPedidos.Infrastructure --startup-project GestaoPedidos.API

echo "Iniciando a aplicação..."
exec "$@"