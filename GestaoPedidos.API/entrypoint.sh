#!/bin/bash
set -e

# Aguardar o banco de dados estar pronto
# (Esta é uma verificação simples, em um ambiente de produção real, use uma ferramenta como wait-for-it.sh)
sleep 15

echo "Aplicando migrações do banco de dados..."
dotnet ef database update --project GestaoPedidos.Infrastructure/GestaoPedidos.Infrastructure.csproj --startup-project GestaoPedidos.API/GestaoPedidos.API.csproj

echo "Iniciando a aplicação..."
dotnet GestaoPedidos.API.dll
