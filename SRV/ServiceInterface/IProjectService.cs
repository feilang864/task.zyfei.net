using System.Collections.Generic;

namespace FFLTask.SRV.ServiceInterface
{
    public interface IProjectService
    {
        #region Get Linked

        LinkedList<FFLTask.SRV.ViewModel.Project.LiteItemModel> GetLinkedProject(int tailProjectId);

        /// <summary>
        /// get the project from one of the the user's root projects
        /// </summary>
        /// <param name="userId">user who has joined some project</param>
        /// <returns>the linked projects from one of the user's root projects(head) to tail</returns>
        FFLTask.SRV.ViewModel.Project._DropdownlistLinkedModel GetDropdownlistLink(int userId);

        /// <summary>
        /// get the whole project link according to the dedicated project(tail selected) and some user
        /// </summary>
        /// <param name="userId">the user has joined the project</param>
        /// <param name="projectId">the tailed(current) project id</param>
        /// <returns>the linked projects from the tailed project to the head</returns>
        FFLTask.SRV.ViewModel.Project._DropdownlistLinkedModel GetDropdownlistLink(int userId, int projectId);

        FFLTask.SRV.ViewModel.Project._DropdownlistLinkedNodeModel GetNextNode(int projectId);
        
        #endregion

        #region Get View related

        IList<FFLTask.SRV.ViewModel.Project.FullItemModel> GetByParent(int parentId);

        FFLTask.SRV.ViewModel.Project._SearchModel GetLinked(int projectId);
        FFLTask.SRV.ViewModel.Project._SearchModel GetByPartialName(string projectName);

        FFLTask.SRV.ViewModel.Project.JoinModel GetJoinTree(int projectId, int currentUserId);

        FFLTask.SRV.ViewModel.Project.EditModel GetEdit(int projectId);
        FFLTask.SRV.ViewModel.Project.SummaryModel GetSummary(int projectId);

        #endregion

        #region Operation

        int Create(FFLTask.SRV.ViewModel.Project.CreateModel model, int? parentId, int userId);
        int Join(int projectId, int userId);
        void Modify(FFLTask.SRV.ViewModel.Project.EditModel model);

        #endregion

        #region Check

        /// <returns>the project's id, 0 is no result</returns>
        int GetByName(string name);
        bool HasChild(int projectId);
        bool HasJoined(int projectId, int userId);
        bool ParentIsOffspring(int childId, int parentId);
        
        #endregion
    }
}
