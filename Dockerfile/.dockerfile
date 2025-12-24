# ================================
# STAGE 1 — Build
# ================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o csproj e restaura dependências
COPY AlugaSe.WebAPI/*.csproj AlugaSe.WebAPI/
RUN dotnet restore AlugaSe.WebAPI/AlugaSe.WebAPI.csproj

# Copia todo o código
COPY . .

# Publica a aplicação
RUN dotnet publish AlugaSe.WebAPI/AlugaSe.WebAPI.csproj -c Release -o /app/publish

# ================================
# STAGE 2 — Runtime
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Render usa a porta 10000 por padrão
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Copia os arquivos publicados
COPY --from=build /app/publish .

# Comando de inicialização
ENTRYPOINT ["dotnet", "AlugaSe.WebAPI.dll"]
