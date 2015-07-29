using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using MessageHub.Lib.UnitOfWork;
using MessageHub.Lib.Service;
using MessageHub.Lib.Repository;
using MessageHub.Lib.Entity;
using Raven.Client;

namespace MessageHub.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            //container.RegisterType<IMessageService, MessageService>(new HierarchicalLifetimeManager());
            //container.RegisterType<ICommentService, CommentService>(new HierarchicalLifetimeManager());
            //container.RegisterType<ILoggingService, LoggingService>(new HierarchicalLifetimeManager());

            //container.RegisterType<IMessageUoW, MessageUoW>(new HierarchicalLifetimeManager());

            //container.RegisterType<IRepository<Message, MessageHubDbContext>, MessageHubRepository<Message>>(new HierarchicalLifetimeManager());
            //container.RegisterType<IRepository<Comment, MessageHubDbContext>, MessageHubRepository<Comment>>(new HierarchicalLifetimeManager());

            container.RegisterType<IMessageService, RavenMessageService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICommentService, RavenCommentService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICategoryService, RavenCategoryService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILoggingService, LoggingService>(new HierarchicalLifetimeManager());
            container.RegisterType<IChatMessageService, RavenChatMessageService>(new HierarchicalLifetimeManager());

            container.RegisterType<IRavenMessageUoW, RavenMessageUoW>(new HierarchicalLifetimeManager());
            container.RegisterType<IRavenCategoryUoW, RavenCategoryUoW>(new HierarchicalLifetimeManager());
            container.RegisterType<IRavenChatMessageUoW, RavenChatMessageUoW>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<Message, IDocumentSession>, MessageRavenRepository<Message, IDocumentSession>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepository<Comment, IDocumentSession>, MessageRavenRepository<Comment, IDocumentSession>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepository<Category, IDocumentSession>, CategoryRavenRepository<Category, IDocumentSession>>(new HierarchicalLifetimeManager());
            container.RegisterType<IChatRepository<ChatMessage, IDocumentSession>, ChatMessageRavenRepository<ChatMessage, IDocumentSession>>(new HierarchicalLifetimeManager());
        }
    }
}
