namespace Gray.DataService.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        //모든 엔티티 반환
        Task<IEnumerable<T>> All();

        //하나의 엔티티만 반환
        Task<T> GetById(Guid id);

        //하나 추가
        Task<bool> Add(T entity);

        //하나 삭제
        Task<bool> Delete(Guid id, string userId);

        //하나 수정
        Task<bool> Edit(Guid id, T entity);
    }
}
