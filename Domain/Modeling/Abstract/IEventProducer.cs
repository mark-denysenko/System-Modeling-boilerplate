
namespace Domain.Modeling.Abstract
{
    public interface IEventProducer
    {
        double CurrentTime { get; set; }
        double NextEventTime { get; set; }
    }
}
