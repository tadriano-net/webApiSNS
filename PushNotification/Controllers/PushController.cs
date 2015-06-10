using System.Web.Http;
using Amazon;
using Amazon.SimpleNotificationService.Model;
using PushNotification.Models;

namespace PushNotification.Controllers
{
    public class PushController : ApiController
    {

        //Os topics arn que nós criamos no painel da AWS

        //O da plataforma
        static string _gcmPlatformArn { get; set; }

        //O nosso tópico
        static string _awsTopicArn { get; set; }

        // GET api/values/5
        public string Post(Receive receive)
        {
            //Colamos os valores aqui e já inicializamos eles.
            _gcmPlatformArn = "#";
            _awsTopicArn = "#";

            //1º Setar a Região que estamos usando na nossa console
            RegionEndpoint regionEndPoint = RegionEndpoint.USEast1;

            //2º Metodo para que possamos acessar as api's da AWS, aqui iremos usar as nossas chaves  awsAccessKey e awsSecretAccessKey
            //e no final da chamada passar a nossa Região, que configuramos acima.

            var snsClient = AWSClientFactory.
                CreateAmazonSimpleNotificationServiceClient("#", "#", regionEndPoint);

            //3º Adicionar o device a plataforma, nesse caso a GCM
            snsClient.CreatePlatformEndpoint(new CreatePlatformEndpointRequest
            {
                CustomUserData = "Android-Device",
                Token = receive.Token,
                PlatformApplicationArn = _gcmPlatformArn,
            });


            //4º Criamos o metodo abaixo para listar dos os devices/endpoints
            //inseridos na nossa plataforma e assinamos eles em um topico nosso.
            ListEndpointsByPlatformApplicationResponse listEndpoints = snsClient.ListEndpointsByPlatformApplication(new ListEndpointsByPlatformApplicationRequest
            {
                PlatformApplicationArn = _gcmPlatformArn
            });


            foreach (var endPoint in listEndpoints.Endpoints)
            {

                snsClient.Subscribe(new SubscribeRequest
                {
                    TopicArn = _awsTopicArn,
                    Protocol = "application",
                    Endpoint = endPoint.EndpointArn
                });
            }



            //5º Agora nós enviamos uma mensagem para o nosso Tópico enviar para todos os devices,
            // que estão assinados nele.
            PublishRequest publishRequest = new PublishRequest()
            {
                Subject = "Blog",
                Message = "O mundo é dos .net",
                TopicArn = _awsTopicArn,
            };

            // por fim enviamos.
            snsClient.Publish(publishRequest);

            return "OK";

        }
    }
}
