namespace CrossCutting.ProcessingPipeline.Tests;

public class ProofOfConceptTests
{
    protected IEnumerable<IPipelineComponent<TRequest>> GetComponents<TRequest>(IPipeline<TRequest> instance)
        => (instance.GetType().GetProperty(nameof(IPipelineBuilder<object>.Components))!.GetValue(instance) as IEnumerable<IPipelineComponent<TRequest>>)!;

    protected IEnumerable<IPipelineComponent<TRequest, TResponse>> GetComponents<TRequest, TResponse>(IPipeline<TRequest, TResponse> instance)
        where TResponse : new()
        => (instance.GetType().GetProperty(nameof(IPipelineBuilder<object>.Components))!.GetValue(instance) as IEnumerable<IPipelineComponent<TRequest, TResponse>>)!;

    public class Pipeline_With_Response : ProofOfConceptTests
    {
        [Fact]
        public void Can_Create_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, StringBuilder>();

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
            var builder = new PipelineBuilder<object?, StringBuilder>();

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
            var builder = new PipelineBuilder<object?, StringBuilder>();

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
            var builder = new PipelineBuilder<object?, StringBuilder>()
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
            var builder = new PipelineBuilder<object?, StringBuilder>()
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
            var builder = new PipelineBuilder<object?, StringBuilder> { Components = null! };
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
            var builder = new PipelineBuilder<object?, StringBuilder> { Components = new List<IBuilder<IPipelineComponent<object?, StringBuilder>>>() };
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
            var builder = new PipelineBuilder<object?, StringBuilder> { Components = new List<IBuilder<IPipelineComponent<object?, StringBuilder>>>(new[] { new MyComponentWithContextBuilder() }) };
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
            PipelineContext<object?, StringBuilder>? context = null;
            Func<PipelineContext<object?, StringBuilder>, Result> processCallback = new(ctx => { context = ctx; ctx.Response.Append("2"); return Result.Continue<object?>(); });
            var sut = new PipelineBuilder<object?, StringBuilder>()
                .AddComponent(new MyComponentWithContextBuilder().WithProcessCallback(processCallback))
                .Build();

            // Act
            var result = await sut.Process(request: 1);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            context.Should().NotBeNull();
            context!.Request.Should().BeEquivalentTo(1);
            result.GetValueOrThrow().ToString().Should().Be("2");
        }

        [Fact]
        public async Task Can_Abort_Pipeline_With_Feature_Using_Non_Success_Status()
        {
            // Arrange
            Func<PipelineContext<object?, StringBuilder>, Result> processCallback = new(_ => Result.Error<object?>("Kaboom"));
            var sut = new PipelineBuilder<object?, StringBuilder>()
                .AddComponent(new MyComponentWithContextBuilder().WithProcessCallback(processCallback))
                .Build();

            // Act
            var result = await sut.Process(request: 1);

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

    public class Pipeline_Without_Response : ProofOfConceptTests
    {
        [Fact]
        public void Can_Create_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?>();

            // Act
            var pipeline = builder
                .AddComponent(new MyResponselessComponentBuilder())
                .Build();

            // Assert
            GetComponents(pipeline).Should().ContainSingle();
            GetComponents(pipeline).Single().Should().BeOfType<MyResponselessComponent>();
        }

        [Fact]
        public void Can_Add_Multiple_Features_Using_Array()
        {
            // Arrange
            var builder = new PipelineBuilder<object?>();

            // Act
            var pipeline = builder
                .AddComponents(new MyResponselessComponentBuilder(), new MyResponselessComponentBuilder())
                .Build();

            // Assert
            GetComponents(pipeline).Should().HaveCount(2);
            GetComponents(pipeline).Should().AllBeOfType<MyResponselessComponent>();
        }

        [Fact]
        public void Can_Add_Multiple_Features_Using_Enumerable()
        {
            // Arrange
            var builder = new PipelineBuilder<object?>();

            // Act
            var pipeline = builder
                .AddComponents(new[] { new MyResponselessComponentBuilder(), new MyResponselessComponentBuilder() }.AsEnumerable())
                .Build();

            // Assert
            GetComponents(pipeline).Should().HaveCount(2);
            GetComponents(pipeline).Should().AllBeOfType<MyResponselessComponent>();
        }

        [Fact]
        public void Can_Replace_Feature_On_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?>()
                .AddComponent(new MyResponselessComponentBuilder());

            // Act
            var pipeline = builder
                .ReplaceComponent<MyResponselessComponentBuilder>(new MyReplacedResponselessComponentBuilder())
                .Build();

            // Assert
            GetComponents(pipeline).Should().ContainSingle();
            GetComponents(pipeline).Single().Should().BeOfType<MyReplacedResponselessComponent>();
        }

        [Fact]
        public void Can_Remove_Feature_On_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?>()
                .AddComponent(new MyResponselessComponentBuilder());

            // Act
            var pipeline = builder
                .RemoveComponent<MyResponselessComponentBuilder>()
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
            var builder = new PipelineBuilder<object?> { Components = new List<IBuilder<IPipelineComponent<object?>>>(new[] { new MyResponselessComponentBuilder() }) };
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
            var builder = new PipelineBuilder<object?> { Components = new List<IBuilder<IPipelineComponent<object?>>>(new[] { new MyResponselessValidatableComponentBuilder().WithProcessCallback(null!) }) };
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
                .AddComponent(new MyResponselessComponentBuilder().WithProcessCallback(processCallback))
                .Build();

            // Act
            var result = await sut.Process(request: 1);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            context.Should().NotBeNull();
            context!.Request.Should().BeEquivalentTo(1);
        }

        [Fact]
        public async Task Can_Abort_Pipeline_With_Feature_Using_Non_Success_Status()
        {
            // Arrange
            Func<PipelineContext<object?>, Result<object?>> processCallback = new(_ => Result.Error<object?>("Kaboom"));
            var sut = new PipelineBuilder<object?>()
                .AddComponent(new MyResponselessComponentBuilder().WithProcessCallback(processCallback))
                .Build();

            // Act
            var result = await sut.Process(request: 1);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Constructing_Pipeline_Using_Null_ValidationDelegate_Throws_ArgumentNullException()
        {
            // Act & Assert
            this.Invoking(_ => new Pipeline<object?, StringBuilder>(validationDelegate: null!, features: Enumerable.Empty<IPipelineComponent<object?, StringBuilder>>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("validationDelegate");
        }

        [Fact]
        public void Constructing_Pipeline_Using_Null_Features_Throws_ArgumentNullException()
        {
            // Act & Assert
            this.Invoking(_ => new Pipeline<object?, StringBuilder>((_, _) => { }, features: null!))
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

    public class PipelineComponent_Without_Context()
    {
        [Fact]
        public async Task Can_Call_Process_Without_CancellationToken()
        {
            // Arrange
            var sut = new MyReplacedResponselessComponent();
            var context = new PipelineContext<object?>(1);

            // Act
            var result = await sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.NotImplemented);
        }
    }

    public class PipelineComponent_With_Context()
    {
        [Fact]
        public async Task Can_Call_Process_Without_CancellationToken()
        {
            // Arrange
            var sut = new MyReplacedComponentWithContext();
            var context = new PipelineContext<object?, StringBuilder>(1, new StringBuilder());

            // Act
            var result = await sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.NotImplemented);
        }
    }

    private sealed class MyResponselessComponent : IPipelineComponent<object?>
    {
        public Func<PipelineContext<object?>, Result> ProcessCallback { get; }

        public MyResponselessComponent()
            => ProcessCallback = new Func<PipelineContext<object?>, Result>(_ => Result.NoContent());

        public MyResponselessComponent(Func<PipelineContext<object?>, Result> processCallback)
            => ProcessCallback = processCallback;

        public Task<Result> Process(PipelineContext<object?> context, CancellationToken token)
            => Task.FromResult(ProcessCallback.Invoke(context));
    }

    private sealed class MyResponselessComponentBuilder : IPipelineComponentBuilder<object?>
    {
        public Func<PipelineContext<object?>, Result<object?>> ProcessCallback { get; set; } = new(_ => Result.NoContent<object?>());

        public MyResponselessComponentBuilder WithProcessCallback(Func<PipelineContext<object?>, Result<object?>> processCallback)
        {
            ProcessCallback = processCallback;
            return this;
        }

        public IPipelineComponent<object?> Build()
            => new MyResponselessComponent(ProcessCallback);
    }

    private sealed class MyResponselessValidatableComponentBuilder : IPipelineComponentBuilder<object?>
    {
        [Required]
        public Func<PipelineContext<object?>, Result<object?>> ProcessCallback { get; set; } = new(_ => Result.NoContent<object?>());

        public MyResponselessValidatableComponentBuilder WithProcessCallback(Func<PipelineContext<object?>, Result<object?>> processCallback)
        {
            ProcessCallback = processCallback;
            return this;
        }

        public IPipelineComponent<object?> Build()
            => new MyResponselessComponent(ProcessCallback);
    }

    private sealed class MyReplacedResponselessComponent : IPipelineComponent<object?>
    {
        public Task<Result> Process(PipelineContext<object?> context, CancellationToken token)
        {
            return Task.FromResult(Result.NotImplemented());
        }
    }

    private sealed class MyReplacedResponselessComponentBuilder : IPipelineComponentBuilder<object?>
    {
        public IPipelineComponent<object?> Build()
            => new MyReplacedResponselessComponent();
    }

    private sealed class MyComponentWithContext : IPipelineComponent<object?, StringBuilder>
    {
        public Func<PipelineContext<object?, StringBuilder>, Result> ProcessCallback { get; }

        public MyComponentWithContext()
            => ProcessCallback = new Func<PipelineContext<object?, StringBuilder>, Result>(_ => Result.NoContent());

        public MyComponentWithContext(Func<PipelineContext<object?, StringBuilder>, Result> processCallback)
            => ProcessCallback = processCallback;

        public Task<Result> Process(PipelineContext<object?, StringBuilder> context, CancellationToken token)
            => Task.FromResult(ProcessCallback.Invoke(context));
    }

    private sealed class MyComponentWithContextBuilder : IPipelineComponentBuilder<object?, StringBuilder>
    {
        public Func<PipelineContext<object?, StringBuilder>, Result> ProcessCallback { get; set; } = new(_ => Result.NoContent());

        public MyComponentWithContextBuilder WithProcessCallback(Func<PipelineContext<object?, StringBuilder>, Result> processCallback)
        {
            ProcessCallback = processCallback;
            return this;
        }

        public IPipelineComponent<object?, StringBuilder> Build()
            => new MyComponentWithContext(ProcessCallback);
    }

    private sealed class MyReplacedComponentWithContext : IPipelineComponent<object?, StringBuilder>
    {
        public Task<Result> Process(PipelineContext<object?, StringBuilder> context, CancellationToken token)
        {
            return Task.FromResult(Result.NotImplemented());
        }
    }

    private sealed class MyReplacedComponentWithContextBuilder : IPipelineComponentBuilder<object?, StringBuilder>
    {
        public IPipelineComponent<object?, StringBuilder> Build()
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
