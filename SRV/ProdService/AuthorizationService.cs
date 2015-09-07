using System;
using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ViewModel.Auth;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.SRV.ViewModelMap;
using NHibernate.Linq;
using FFLTask.SRV.Query;

namespace FFLTask.SRV.ProdService
{
    public class AuthorizationService : BaseService, ServiceInterface.IAuthroizationService
    {
        private IQueryable<Authorization> _querySource;
        public AuthorizationService()
        {
            _querySource = session.Query<Authorization>();
        }

        public IList<Token> GetTokens(int userId, int projectId)
        {
            IList<Token> tokens = new List<Token>();

            User user = session.Load<User>(userId);
            Authorization auth = user.Authorizations.Where(a => a.Project.Id == projectId).SingleOrDefault();

            if (auth != null)
            {
                if (auth.IsFounder)
                {
                    tokens.Add(Token.Founder);
                }
                if (auth.IsPublisher)
                {
                    tokens.Add(Token.Assinger);
                }
                if (auth.IsOwner)
                {
                    tokens.Add(Token.Owner);
                }
            }

            return tokens;
        }

        public IList<ProjectMemberModel> Get(int projectId)
        {
            throw new NotImplementedException();
        }

        public IList<ProjectAuthorizationModel> GetAdmined(int userId)
        {
            IList<ProjectAuthorizationModel> models = new List<ProjectAuthorizationModel>();

            User result = session.Load<User>(userId);
            IList<Authorization> isAdminAuthorizations = result.Authorizations
                .Where(x => x.IsAdmin).ToList();


            models.FilledBy(isAdminAuthorizations);

            return models;
        }

        public void Update(AuthorizationModel model)
        {
            Authorization auth = session.Load<Authorization>(model.Id);
            auth.IsAdmin = model.CanAdmin;
            auth.IsOwner = model.CanOwn;
            auth.IsPublisher = model.CanPublish;
        }

        public void Dismiss(int userId, IList<int> projectIds)
        {
            var auths = _querySource.WithUser(userId).InProjects(projectIds);
            foreach (var auth in auths)
            {
                session.Delete(auth);
            }
        }

        public IList<int> GetAdminedProjectIds(int userId)
        {
            return session.Load<User>(userId).Authorizations
                .Where(u => u.IsAdmin)
                .Select(u => u.Project.Id)
                .ToList<int>();
        }
    }
}
