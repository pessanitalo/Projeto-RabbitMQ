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
        canal.ExchangeDeclare(exchange: "mensagem_empresa", type: ExchangeType.Direct);

        Console.WriteLine("Digite uma mensagem");

        while (true)
        {
            string mensagem = Console.ReadLine();

            if (mensagem == "!finalizar") break;

            var corpoMensagem = Encoding.UTF8.GetBytes(mensagem);

            //pegar uma routkey
            Console.WriteLine("Digite uma rota");
            string rotaKey = Console.ReadLine();

            canal.BasicPublish(exchange: "mensagem_empresa",
                                 routingKey: rotaKey,
                                 basicProperties: null,
                                 body: corpoMensagem);

            Console.WriteLine(" [x] Enviou {0}", mensagem);
        }
    }

    Console.WriteLine(" Pressione [enter] para sair.");
    Console.ReadLine();
}
