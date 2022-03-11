using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using PH.ClientName2.Data;

namespace PH.ClientName2.Common
{
    public class PluginAbstract<T> where T : Entity
    {
        #region Declaracion de variables globales

        public IPluginExecutionContext context = null;
        public IOrganizationService iServices = null;
        public ITracingService tracingService = null;
        public XrmDataContext svcContext = null;
        private IOrganizationServiceFactory factory = null;

        public string relationshipName = string.Empty;

        #endregion

        protected void InitializeServices(IServiceProvider serviceProvider)
        {
            context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            iServices = factory.CreateOrganizationService(context.UserId);
            tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            svcContext = new XrmDataContext(iServices);
        }

        protected T GetTargetEntity()
        {
            if (!context.InputParameters.Contains("Target"))
            {
                //throw new ArgumentException("No fué posible obtener el target del contexto de ejecución.");
                return null;
            }

            return ((Entity)context.InputParameters["Target"]).ToEntity<T>();
        }

        protected Guid GetGuidFromEntity(string parameter)
        {
            if (!context.InputParameters.Contains(parameter))
            {
                throw new ArgumentException("No fué posible obtener el target del contexto de ejecución.");
            }
            EntityReference reference = (EntityReference)context.InputParameters[parameter];
            return reference.Id;
        }

        protected T GetEntity(string entity)
        {
            if (!context.InputParameters.Contains(entity))
            {
                //throw new ArgumentException("No fué posible obtener el target del contexto de ejecución.");
                return null;
            }

            return ((Entity)context.InputParameters[entity]).ToEntity<T>();
        }

        protected T GetPreImageEntity(string preImageName)
        {
            if (!context.PreEntityImages.Contains(preImageName))
            {
                throw new ArgumentException("No fué posible obtener la PreImagen: " + preImageName);
            }
            return context.PreEntityImages[preImageName].ToEntity<T>();
        }

        protected T GetPostImageEntity(string postImageName)
        {
            if (!context.PostEntityImages.Contains(postImageName))
            {
                throw new ArgumentException("No fué posible obtener la PostImagen: " + postImageName);
            }
            return context.PostEntityImages[postImageName].ToEntity<T>();
        }
    }

}
