# Backend Challange
  

### Como rodar

Instruções passo a passo sobre como configurar o ambiente de desenvolvimento local:

1. Para rodar o projeto você precisa ter um ambiente .NET configurado, o .NET usado foi a versão 8.0
2. Você precisará também do RabbitMQ e o Postgres, se não tiver você pode instala-los ou rodar via docker.
3. Nesse projeto está configurado um docker-compose, você pode ir na raíz do projeto e usar o comando 'docker-compose up --build'e isso fará com que o projeto
    rode normalmente.
 
### Comandos docker para rodar sem o docker-compose 

1. -  docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management
2. -  docker run --name my-postgres -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -v my_pgdata:/var/lib/postgresql/data -d postgres 

### Observações

1. - Ao rodar a primeira vez o sistema, o sistema criará um usuário administrador 
      padrão: admin@example.com 
      Admin@123
  
### Contato
1. Qualquer dúvida vocês podem entrar em contato comigo a qualquer momento: (61)982194417