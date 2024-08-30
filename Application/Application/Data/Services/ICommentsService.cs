using Application.Models;

namespace Application.Data.Services
{
    public interface ICommentsService
    {
        Task Add(Comment comment);
    }
}
