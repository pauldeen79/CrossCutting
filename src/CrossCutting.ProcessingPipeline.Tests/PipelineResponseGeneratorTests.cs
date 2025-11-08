namespace CrossCutting.ProcessingPipeline.Tests;

public class PipelineResponseGeneratorTests : TestBase
{
    public class Generate : PipelineResponseGeneratorTests
    {
        [Fact]
        public void Returns_Custom_Result_When_Component_Is_Registered_For_Specified_Command()
        {
            // Arrange
            var responseGeneratorComponent = Fixture.Create<IPipelineResponseGeneratorComponent>();
            responseGeneratorComponent
                .Generate<Generate>(Arg.Any<object>())
                .Returns(Result.Success(this));
            var sut = new PipelineResponseGenerator([responseGeneratorComponent]);

            // Act
            var result = sut.Generate<Generate>(new object());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeSameAs(this);
        }

        [Fact]
        public void Returns_Ok_Result_With_New_Instance_Using_Default_Constructor_When_No_Component_Is_Registered_For_Specified_Command()
        {
            // Arrange
            var sut = new PipelineResponseGenerator([]);

            // Act
            var result = sut.Generate<Generate>(new object());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeSameAs(this);
        }

        [Fact]
        public void Returns_NotSupported_Result_When_No_Component_Is_Registered_For_Specified_Command_And_Response_Does_Not_Have_A_Default_Constructor()
        {
            // Arrange
            var sut = new PipelineResponseGenerator([]);

            // Act
            var result = sut.Generate<ClassWithoutPublicConstructor>(new object());

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
            result.ErrorMessage.ShouldBe("Response of type CrossCutting.ProcessingPipeline.Tests.PipelineResponseGeneratorTests+Generate+ClassWithoutPublicConstructor could not be constructed");
        }

        [Fact]
        public void Returns_NotSupported_Result_When_No_Component_Is_Registered_For_Specified_Command_And_Response_Is_Abstract_Class()
        {
            // Arrange
            var sut = new PipelineResponseGenerator([]);

            // Act
            var result = sut.Generate<AbstractClass>(new object());

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
            result.ErrorMessage.ShouldBe("Response of type CrossCutting.ProcessingPipeline.Tests.PipelineResponseGeneratorTests+Generate+AbstractClass could not be constructed");
        }

        [Fact]
        public void Returns_NotSupported_Result_When_No_Component_Is_Registered_For_Specified_Command_And_Response_Is_Interface()
        {
            // Arrange
            var sut = new PipelineResponseGenerator([]);

            // Act
            var result = sut.Generate<IMyInterface>(new object());

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
            result.ErrorMessage.ShouldBe("Response of type CrossCutting.ProcessingPipeline.Tests.PipelineResponseGeneratorTests+Generate+IMyInterface could not be constructed");
        }

        public interface IMyInterface
        {
        }

#pragma warning disable S2094 // Classes should not be empty
        public abstract class AbstractClass : IMyInterface
#pragma warning restore S2094 // Classes should not be empty
        {
        }

        public class ClassWithoutPublicConstructor : AbstractClass
        {
            public ClassWithoutPublicConstructor(int value)
            {
            }
        }
    }
}
