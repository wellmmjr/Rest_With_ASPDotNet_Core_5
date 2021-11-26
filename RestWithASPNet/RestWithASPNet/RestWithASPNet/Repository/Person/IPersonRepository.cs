namespace RestWithASPNet.Repository.Person
{
    public interface IPersonRepository : IRepository<Model.Person>
    {
       Model.Person ActiveUser(long id);
    }
}
