using Structurizr;
using Structurizr.Api;

namespace c4_model_design
{
    class Program
    {
        static void Main(string[] args)
        {
            Banking();
        }

        static void Banking()
        {
            const long workspaceId = 71007;
            const string apiKey = "37b7962a-1f24-4f9f-b70f-5810f51543d6";
            const string apiSecret = "37d0c8a7-5efb-42c1-9027-85f0c61630a1";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("C4 Model - Sistema Adopcion de mascotas", "Sistema Adopcion de mascotas");
            ViewSet viewSet = workspace.Views;
            Model model = workspace.Model;

            // 1. Diagrama de Contexto
            SoftwareSystem adoptionSystem = model.AddSoftwareSystem("Monitoreo del adopcion de mascotas", "Permite el seguimiento y monitoreo de la adopcion de mascotas.");
            SoftwareSystem payU = model.AddSoftwareSystem("PayU", "API de PayU le permite a tu negocio procesar transacciones desde diferentes tipos de aplicaciones.");

            Person usuarioAdmin = model.AddPerson("Usuario Admin", "Ciudadano peruano que registra y asigna usuarios colaboradores para las solicitudes de adopcion.");
            Person usuarioColaborador = model.AddPerson("Usuario Colaborador", "Ciudadano peruano que realiza solicitudes de adopcion");
            Person usuarioAdoptante = model.AddPerson("Usuario Adoptante", "Ciudadano peruano que atiende las solicitudes de adopcion");
            
            usuarioAdmin.Uses(adoptionSystem, "Consulta en todo momento el seguimiento del estado de una solicitud de adopción.");
            usuarioAdoptante.Uses(adoptionSystem, "Consulta en todo momento el seguimiento del estado de una solicitud de adopción.");
            usuarioColaborador.Uses(adoptionSystem, "Consulta en todo momento el seguimiento del estado de una solicitud de adopción.");
            adoptionSystem.Uses(payU, "Usa la API de PayU para realizar el pago de la contribución monetaria correspondiente");
            
            SystemContextView contextView = viewSet.CreateSystemContextView(adoptionSystem, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A5_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // Tags
            usuarioAdmin.AddTags("Usuario");
            usuarioAdoptante.AddTags("Usuario");
            usuarioColaborador.AddTags("Usuario");
            adoptionSystem.AddTags("SistemaAdopcion");
            payU.AddTags("payU");

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("Usuario") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("SistemaAdopcion") { Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("payU") { Background = "#90714c", Color = "#ffffff", Shape = Shape.RoundedBox });

            // 2. Diagrama de Contenedores
            Container mobileApplication = adoptionSystem.AddContainer("Mobile App", "Permite a los usuarios visualizar en todo momento el seguimiento del estado de una solicitud de adopción.", "Kotlin");
            Container webApplication = adoptionSystem.AddContainer("Web App", "Permite a los usuarios visualizar en todo momento el seguimiento del estado de una solicitud de adopción.", "Vue.js");
            Container landingPage = adoptionSystem.AddContainer("Landing Page", "", "HTML y CSS");
            Container apiRest = adoptionSystem.AddContainer("API Rest", "API Rest", "Asp.net Core");

            Container collaboratorContext = adoptionSystem.AddContainer("Collaborator Context", "Bounded Context del Microservicio de Gestion de colaboradores", "NodeJS (NestJS)");
            Container petsContext = adoptionSystem.AddContainer("Pets Context", "Bounded Context del Microservicio de información de mascotas", "NodeJS (NestJS)");
            Container solicitudeContext = adoptionSystem.AddContainer("Solicitude Context", "Bounded Context del Microservicio de Solicitud de adopcion", "NodeJS (NestJS)");
            Container monitoringContext = adoptionSystem.AddContainer("Monitoring Context", "Bounded Context del Microservicio de Monitoreo en tiempo real del estado de una solicitud de adopcion", "NodeJS (NestJS)");

            Container database = adoptionSystem.AddContainer("Database", "", "MySQL");
            
            usuarioAdmin.Uses(mobileApplication, "Consulta");
            usuarioAdmin.Uses(webApplication, "Consulta");
            usuarioAdmin.Uses(landingPage, "Consulta");
            usuarioAdoptante.Uses(mobileApplication, "Consulta");
            usuarioAdoptante.Uses(webApplication, "Consulta");
            usuarioAdoptante.Uses(landingPage, "Consulta");
            usuarioColaborador.Uses(mobileApplication, "Consulta");
            usuarioColaborador.Uses(webApplication, "Consulta");
            usuarioColaborador.Uses(landingPage, "Consulta");

            mobileApplication.Uses(apiRest, "API Request", "JSON/HTTPS");
            webApplication.Uses(apiRest, "API Request", "JSON/HTTPS");

            apiRest.Uses(collaboratorContext, "", "");
            apiRest.Uses(petsContext, "", "");
            apiRest.Uses(solicitudeContext, "", "");
            apiRest.Uses(monitoringContext, "", "");
            
            collaboratorContext.Uses(database, "", "JDBC");
            petsContext.Uses(database, "", "JDBC");
            solicitudeContext.Uses(database, "", "JDBC");
            monitoringContext.Uses(database, "", "JDBC");
            
            monitoringContext.Uses(payU, "API Request", "JSON/HTTPS");

            // Tags
            mobileApplication.AddTags("MobileApp");
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiRest.AddTags("APIRest");
            database.AddTags("Database");
            collaboratorContext.AddTags("collaboratorContext");
            petsContext.AddTags("PetsContext");
            solicitudeContext.AddTags("SolicitudeContext");
            monitoringContext.AddTags("MonitoringContext");

            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIRest") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("Database") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("collaboratorContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PetsContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SolicitudeContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });

            ContainerView containerView = viewSet.CreateContainerView(adoptionSystem, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();            

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}