using RabbitMQ.Client;
using System.Text;

var servidor = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 5672,
    UserName = "usuario",
    Password = "senha@123",
};

var conexao = servidor.CreateConnection();
{
  
    using (var canal = conexao.CreateModel())
    {
     
        canal.QueueDeclare(queue: "ola",
                      durable: false,
                      exclusive: false,
                      autoDelete: false,
                      arguments: null);

    
        string mensagem = "Olá Mundo!";
     
        var corpoMensagem = Encoding.UTF8.GetBytes(mensagem);

        canal.BasicPublish(exchange: "",
                             routingKey: "ola",
                             basicProperties: null,
                             body: corpoMensagem);
        Console.WriteLine(" [x] Enviou {0}", mensagem);
    }

    Console.WriteLine(" Pressione [enter] para sair.");
    Console.ReadLine();
}
