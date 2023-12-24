using Xunit;

namespace KataStarter.Tests;

public class GreeterShould
{
    [Fact]
    public void say_hello()
    {
        var greeter = new Greeter();

        var result = greeter.Greet("Pedro");

        Assert.Equal("¡Hola, Pedro!", result);
    }
}