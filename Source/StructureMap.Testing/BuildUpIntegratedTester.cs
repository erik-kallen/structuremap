using NUnit.Framework;
using StructureMap.Graph;
using StructureMap.Testing.Widget3;

namespace StructureMap.Testing
{
    [TestFixture]
    public class BuildUpIntegratedTester
    {
        [SetUp]
        public void SetUp()
        {
            PluginCache.ResetAll();
        }

        [Test]
        public void create_a_setter_rule_and_see_it_applied_in_BuildUp()
        {
            var theGateway = new DefaultGateway();
            var container = new Container(x =>
            {
                x.ForRequestedType<IGateway>().TheDefault.IsThis(theGateway);
                x.SetAllProperties(y =>
                {
                    y.OfType<IGateway>();
                });
            });

            var target = new BuildUpTarget1();
            container.BuildUp(target);

            target.Gateway.ShouldBeTheSameAs(theGateway);
            target.Service.ShouldBeNull();
        }

        [Test]
        public void use_predefined_setter_values_for_buildup()
        {
            var theGateway = new DefaultGateway();
            var container = new Container(x =>
            {
                x.ForConcreteType<BuildUpTarget1>().Configure
                    .SetterDependency(y => y.Gateway).Is(theGateway);
            });

            var target = new BuildUpTarget1();
            container.BuildUp(target);

            target.Gateway.ShouldBeTheSameAs(theGateway);
        }

        [Test]
        public void create_a_setter_rule_and_see_it_applied_in_BuildUp_through_ObjectFactory()
        {
            var theGateway = new DefaultGateway();
            ObjectFactory.Initialize(x =>
            {
                x.IgnoreStructureMapConfig = true;
                x.ForRequestedType<IGateway>().TheDefault.IsThis(theGateway);
                x.SetAllProperties(y =>
                {
                    y.OfType<IGateway>();
                });
            });

            var target = new BuildUpTarget1();
            ObjectFactory.BuildUp(target);

            target.Gateway.ShouldBeTheSameAs(theGateway);
        }
    }

    public class BuildUpTarget1
    {
        public IGateway Gateway { get; set; }
        public IService Service { get; set; }
    }
}