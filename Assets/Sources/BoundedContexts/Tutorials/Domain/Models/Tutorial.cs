using System;
using Sources.BoundedContexts.Ids;
using Sources.BoundedContexts.Ids.Domain.Constant;
using Sources.Domain.Models.Data;
using Sources.Frameworks.Domain.Interfaces.Entities;

namespace Sources.BoundedContexts.Tutorials.Domain
{
    public class Tutorial : IEntity
    {
        public Tutorial(TutorialDto tutorialDto)
            : this(tutorialDto.Id, tutorialDto.HasCompleted)
        {
        }

        public Tutorial()
            : this(ModelId.Tutorial, false)
        {
        }

        private Tutorial(string id, bool hasCompleted)
        {
            Id = id;
            HasCompleted = hasCompleted;
        }

        public bool HasCompleted { get; set; }

        public string Id { get; }

        public Type Type => GetType();
    }
}