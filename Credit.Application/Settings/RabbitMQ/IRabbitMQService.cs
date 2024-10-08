namespace Credit.Application.Settings.RabbitMQ
{
    public interface IRabbitMQService
    {
        public void BasicPublish(object message);
        public IEnumerable<T> BasicGet<T>();
    }
}
