namespace CrossCutting.ProcessingPipeline.Tests;

public class ProofOfConceptTests
{
    protected IEnumerable<IPipelineComponent<TModel>> GetComponents<TModel>(IPipeline<TModel> instance)
        => (instance.GetType().GetProperty(nameof(IPipelineBuilder<object>.Components))!.GetValue(instance) as IEnumerable<IPipelineComponent<TModel>>)!;

    protected IEnumerable<IPipelineComponent<TModel, TContext>> GetComponents<TModel, TContext>(IPipeline<TModel, TContext> instance)
        => (instance.GetType().GetProperty(nameof(IPipelineBuilder<object>.Components))!.GetValue(instance) as IEnumerable<IPipelineComponent<TModel, TContext>>)!;

    public class Pipeline_With_Context : ProofOfConceptTests
    {
        [Fact]
        public void Can_Create_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?>();

            // Act
            var pipeline = builder
                .AddComponent(new MyComponentWithContextBuilder())
                .Build();

            // Assert
            GetComponents(pipeline).Should().ContainSingle();
            GetComponents(pipeline).Single().Should().BeOfType<MyComponentWithContext>();
        }

        [Fact]
        public void Can_Add_Multiple_Features_Using_Array()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?>();

            // Act
            var pipeline = builder
                .AddComponents(new MyComponentWithContextBuilder(), new MyComponentWithContextBuilder())
                .Build();

            // Assert
            GetComponents(pipeline).Should().HaveCount(2);
            GetComponents(pipeline).Should().AllBeOfType<MyComponentWithContext>();
        }

        [Fact]
        public void Can_Add_Multiple_Features_Using_Enumerable()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?>();

            // Act
            var pipeline = builder
                .AddComponents(new[] { new MyComponentWithContextBuilder(), new MyComponentWithContextBuilder() }.AsEnumerable())
                .Build();

            // Assert
            GetComponents(pipeline).Should().HaveCount(2);
            GetComponents(pipeline).Should().AllBeOfType<MyComponentWithContext>();
        }

        [Fact]
        public void Can_Replace_Feature_On_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?>()
                .AddComponent(new MyComponentWithContextBuilder());

            // Act
            var pipeline = builder
                .ReplaceComponent<MyComponentWithContextBuilder>(new MyReplacedComponentWithContextBuilder())
                .Build();

            // Assert
            GetComponents(pipeline).Should().ContainSingle();
            GetComponents(pipeline).Single().Should().BeOfType<MyReplacedComponentWithContext>();
        }

        [Fact]
        public void Can_Remove_Feature_On_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?>()
                .AddComponent(new MyComponentWithContextBuilder());

            // Act
            var pipeline = builder
                .RemoveComponent<MyComponentWithContextBuilder>()
                .Build();

            // Assert
            GetComponents(pipeline).Should().BeEmpty();
        }

        [Fact]
        public void Can_Validate_PipelineBuilder_With_Null_Features_List()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?> { Components = null! };
            var validationResults = new List<ValidationResult>();

            // Act
            _ = builder.TryValidate(validationResults);

            // Assert
            validationResults.Should().ContainSingle();
        }

        [Fact]
        public void Can_Validate_PipelineBuilder_With_Empty_Features_List()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?> { Components = new List<IBuilder<IPipelineComponent<object?, object?>>>() };
            var validationResults = new List<ValidationResult>();

            // Act
            _ = builder.TryValidate(validationResults);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void Can_Validate_PipelineBuilder_With_NonEmpty_Features_List()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?> { Components = new List<IBuilder<IPipelineComponent<object?, object?>>>(new[] { new MyComponentWithContextBuilder() }) };
            var validationResults = new List<ValidationResult>();

            // Act
            _ = builder.TryValidate(validationResults);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public async Task Can_Process_Pipeline_With_Feature()
        {
            // Arrange
            PipelineContext<object?, object?>? context = null;
            Func<PipelineContext<object?, object?>, Result<object?>> processCallback = new(ctx => { context = ctx; return Result.Continue<object?>(); });
            var sut = new PipelineBuilder<object?, object?>()
                .AddComponent(new MyComponentWithContextBuilder().WithProcessCallback(processCallback))
                .Build();

            // Act
            var result = await sut.Process(model: 1, context: 2);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            context.Should().NotBeNull();
            context!.Model.Should().BeEquivalentTo(1);
            context.Context.Should().BeEquivalentTo(2);
        }

        [Fact]
        public async Task Can_Abort_Pipeline_With_Feature_Using_Non_Success_Status()
        {
            // Arrange
            Func<PipelineContext<object?, object?>, Result<object?>> processCallback = new(_ => Result.Error<object?>("Kaboom"));
            var sut = new PipelineBuilder<object?, object?>()
                .AddComponent(new MyComponentWithContextBuilder().WithProcessCallback(processCallback))
                .Build();

            // Act
            var result = await sut.Process(model: 1, context: 2);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Constructing_Pipeline_Using_Null_ValidationDelegate_Throws_ArgumentNullException()
        {
            // Act & Assert
            this.Invoking(_ => new Pipeline<object?>(validationDelegate: null!, features: Enumerable.Empty<IPipelineComponent<object?>>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("validationDelegate");
        }

        [Fact]
        public void Constructing_Pipeline_Using_Null_Features_Throws_ArgumentNullException()
        {
            // Act & Assert
            this.Invoking(_ => new Pipeline<object?>((_, _) => { }, features: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("features");
        }
    }

    public class Pipeline_Without_Context : ProofOfConceptTests
    {
        [Fact]
        public void Can_Create_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?>();

            // Act
            var pipeline = builder
                .AddComponent(new MyContextlessComponentBuilder())
                .Build();

            // Assert
            GetComponents(pipeline).Should().ContainSingle();
            GetComponents(pipeline).Single().Should().BeOfType<MyContextlessComponent>();
        }

        [Fact]
        public void Can_Add_Multiple_Features_Using_Array()
        {
            // Arrange
            var builder = new PipelineBuilder<object?>();

            // Act
            var pipeline = builder
                .AddComponents(new MyContextlessComponentBuilder(), new MyContextlessComponentBuilder())
                .Build();

            // Assert
            GetComponents(pipeline).Should().HaveCount(2);
            GetComponents(pipeline).Should().AllBeOfType<MyContextlessComponent>();
        }

        [Fact]
        public void Can_Add_Multiple_Features_Using_Enumerable()
        {
            // Arrange
            var builder = new PipelineBuilder<object?>();

            // Act
            var pipeline = builder
                .AddComponents(new[] { new MyContextlessComponentBuilder(), new MyContextlessComponentBuilder() }.AsEnumerable())
                .Build();

            // Assert
            GetComponents(pipeline).Should().HaveCount(2);
            GetComponents(pipeline).Should().AllBeOfType<MyContextlessComponent>();
        }

        [Fact]
        public void Can_Replace_Feature_On_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?>()
                .AddComponent(new MyContextlessComponentBuilder());

            // Act
            var pipeline = builder
                .ReplaceComponent<MyContextlessComponentBuilder>(new MyReplacedContextlessComponentBuilder())
                .Build();

            // Assert
            GetComponents(pipeline).Should().ContainSingle();
            GetComponents(pipeline).Single().Should().BeOfType<MyReplacedContextlessComponent>();
        }

        [Fact]
        public void Can_Remove_Feature_On_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?>()
                .AddComponent(new MyContextlessComponentBuilder());

            // Act
            var pipeline = builder
                .RemoveComponent<MyContextlessComponentBuilder>()
                .Build();

            // Assert
            GetComponents(pipeline).Should().BeEmpty();
        }

        [Fact]
        public void Can_Validate_PipelineBuilder_With_Null_Features_List()
        {
            // Arrange
            var builder = new PipelineBuilder<object?> { Components = null! };
            var validationResults = new List<ValidationResult>();

            // Act
            _ = builder.TryValidate(validationResults);

            // Assert
            validationResults.Should().ContainSingle();
        }

        [Fact]
        public void Can_Validate_PipelineBuilder_With_Empty_Features_List()
        {
            // Arrange
            var builder = new PipelineBuilder<object?> { Components = new List<IBuilder<IPipelineComponent<object?>>>() };
            var validationResults = new List<ValidationResult>();

            // Act
            _ = builder.TryValidate(validationResults);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void Can_Validate_PipelineBuilder_With_NonEmpty_Features_List()
        {
            // Arrange
            var builder = new PipelineBuilder<object?> { Components = new List<IBuilder<IPipelineComponent<object?>>>(new[] { new MyContextlessComponentBuilder() }) };
            var validationResults = new List<ValidationResult>();

            // Act
            _ = builder.TryValidate(validationResults);

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void Can_Validate_PipelineBuilder_With_NonEmpty_Features_List_Validatable_Component()
        {
            // Arrange
            var builder = new PipelineBuilder<object?> { Components = new List<IBuilder<IPipelineComponent<object?>>>(new[] { new MyContextlessValidatableComponentBuilder().WithProcessCallback(null!) }) };
            var validationResults = new List<ValidationResult>();

            // Act
            _ = builder.TryValidate(validationResults);

            // Assert
            validationResults.Should().ContainSingle();
            validationResults.Single().ErrorMessage.Should().Be("The field Components is invalid.");
        }

        [Fact]
        public async Task Can_Process_Pipeline_With_Feature()
        {
            // Arrange
            PipelineContext<object?>? context = null;
            Func<PipelineContext<object?>, Result<object?>> processCallback = new(ctx => { context = ctx; return Result.Continue<object?>(); });
            var sut = new PipelineBuilder<object?>()
                .AddComponent(new MyContextlessComponentBuilder().WithProcessCallback(processCallback))
                .Build();

            // Act
            var result = await sut.Process(model: 1);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            context.Should().NotBeNull();
            context!.Model.Should().BeEquivalentTo(1);
        }

        [Fact]
        public async Task Can_Abort_Pipeline_With_Feature_Using_Non_Success_Status()
        {
            // Arrange
            Func<PipelineContext<object?>, Result<object?>> processCallback = new(_ => Result.Error<object?>("Kaboom"));
            var sut = new PipelineBuilder<object?>()
                .AddComponent(new MyContextlessComponentBuilder().WithProcessCallback(processCallback))
                .Build();

            // Act
            var result = await sut.Process(model: 1);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Constructing_Pipeline_Using_Null_ValidationDelegate_Throws_ArgumentNullException()
        {
            // Act & Assert
            this.Invoking(_ => new Pipeline<object?, object?>(validationDelegate: null!, features: Enumerable.Empty<IPipelineComponent<object?, object?>>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("validationDelegate");
        }

        [Fact]
        public void Constructing_Pipeline_Using_Null_Features_Throws_ArgumentNullException()
        {
            // Act & Assert
            this.Invoking(_ => new Pipeline<object?, object?>((_, _) => { }, features: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("features");
        }
    }

    public class Pipeline_With_Dependencies
    {
        [Fact]
        public void Can_Use_Dependency_Injection_To_Allow_Features_To_Have_Dependencies()
        {
            // Arrange
            using var provider = new ServiceCollection()
                .AddScoped<IMyService, MyService>()
                .AddScoped<IPipelineFeatureBuilderWithDependencies, PipelineComponentBuilderWithDependencies>()
                .BuildServiceProvider();
            using var scope = provider.CreateScope();

            // Act
            var sut = scope.ServiceProvider.GetRequiredService<IPipelineFeatureBuilderWithDependencies>();

            // Assert
            sut.MyService.Should().NotBeNull();
        }
    }

    private sealed class MyContextlessComponent : IPipelineComponent<object?>
    {
        public Func<PipelineContext<object?>, Result<object?>> ProcessCallback { get; }

        public MyContextlessComponent()
            => ProcessCallback = new Func<PipelineContext<object?>, Result<object?>>(_ => Result.NoContent<object?>());

        public MyContextlessComponent(Func<PipelineContext<object?>, Result<object?>> processCallback)
            => ProcessCallback = processCallback;

        public Task<Result<object?>> Process(PipelineContext<object?> context, CancellationToken token)
            => Task.FromResult(ProcessCallback.Invoke(context));
    }

    private sealed class MyContextlessComponentBuilder : IPipelineComponentBuilder<object?>
    {
        public Func<PipelineContext<object?>, Result<object?>> ProcessCallback { get; set; } = new(_ => Result.NoContent<object?>());

        public MyContextlessComponentBuilder WithProcessCallback(Func<PipelineContext<object?>, Result<object?>> processCallback)
        {
            ProcessCallback = processCallback;
            return this;
        }

        public IPipelineComponent<object?> Build()
            => new MyContextlessComponent(ProcessCallback);
    }

    private sealed class MyContextlessValidatableComponentBuilder : IPipelineComponentBuilder<object?>
    {
        [Required]
        public Func<PipelineContext<object?>, Result<object?>> ProcessCallback { get; set; } = new(_ => Result.NoContent<object?>());

        public MyContextlessValidatableComponentBuilder WithProcessCallback(Func<PipelineContext<object?>, Result<object?>> processCallback)
        {
            ProcessCallback = processCallback;
            return this;
        }

        public IPipelineComponent<object?> Build()
            => new MyContextlessComponent(ProcessCallback);
    }

    private sealed class MyReplacedContextlessComponent : IPipelineComponent<object?>
    {
        public Task<Result<object?>> Process(PipelineContext<object?> context, CancellationToken token)
        {
            return Task.FromResult(Result.NotImplemented<object?>());
        }
    }

    private sealed class MyReplacedContextlessComponentBuilder : IPipelineComponentBuilder<object?>
    {
        public IPipelineComponent<object?> Build()
            => new MyReplacedContextlessComponent();
    }

    private sealed class MyComponentWithContext : IPipelineComponent<object?, object?>
    {
        public Func<PipelineContext<object?, object?>, Result<object?>> ProcessCallback { get; }

        public MyComponentWithContext()
            => ProcessCallback = new Func<PipelineContext<object?, object?>, Result<object?>>(_ => Result.NoContent<object?>());

        public MyComponentWithContext(Func<PipelineContext<object?, object?>, Result<object?>> processCallback)
            => ProcessCallback = processCallback;

        public Task<Result<object?>> Process(PipelineContext<object?, object?> context, CancellationToken token)
            => Task.FromResult(ProcessCallback.Invoke(context));
    }

    private sealed class MyComponentWithContextBuilder : IPipelineComponentBuilder<object?, object?>
    {
        public Func<PipelineContext<object?, object?>, Result<object?>> ProcessCallback { get; set; } = new(_ => Result.NoContent<object?>());

        public MyComponentWithContextBuilder WithProcessCallback(Func<PipelineContext<object?, object?>, Result<object?>> processCallback)
        {
            ProcessCallback = processCallback;
            return this;
        }

        public IPipelineComponent<object?, object?> Build()
            => new MyComponentWithContext(ProcessCallback);
    }

    private sealed class MyReplacedComponentWithContext : IPipelineComponent<object?, object?>
    {
        public Task<Result<object?>> Process(PipelineContext<object?, object?> context, CancellationToken token)
        {
            return Task.FromResult(Result.NotImplemented<object?>());
        }
    }

    private sealed class MyReplacedComponentWithContextBuilder : IPipelineComponentBuilder<object?, object?>
    {
        public IPipelineComponent<object?, object?> Build()
            => new MyReplacedComponentWithContext();
    }

    private interface IPipelineFeatureBuilderWithDependencies : IPipelineComponentBuilder<object?>
    {
        IMyService MyService { get; }
    }

    public sealed class PipelineComponentBuilderWithDependencies : IPipelineFeatureBuilderWithDependencies
    {
        private readonly IMyService _myService;

        public PipelineComponentBuilderWithDependencies(IMyService myService)
        {
            _myService = myService;
        }

        public IPipelineComponent<object?> Build()
        {
            throw new NotImplementedException();
        }

        public IMyService MyService => _myService;
    }

    public interface IMyService
    {
        void DoSomething();
    }

    private sealed class MyService : IMyService
    {
        public void DoSomething() => throw new NotImplementedException();
    }
}
