using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DevDistricts;
using DevDistricts.Internal;
using Xunit;

[District]
[Occupant(UserName = "test", MachineName = "box")]
internal class SingleMatchDistrict {}

[District]
[Occupant(UserName = "amb", MachineName = "box")]
internal class AmbiguousDistrictA {}

[District]
[Occupant(UserName = "amb", MachineName = "box")]
internal class AmbiguousDistrictB {}

public class InternalExtensionsTests
{
    [Fact]
    public void MatchDistrict_Returns_Matching_Type()
    {
        var districts = Assembly.GetExecutingAssembly().GetDistricts();
        var match = districts.MatchDistrict("test", "box");
        Assert.NotNull(match);
        Assert.Equal(typeof(SingleMatchDistrict), match.Value.Type);
    }

    [Fact]
    public void MatchDistrict_Returns_Null_When_No_Match()
    {
        var districts = Assembly.GetExecutingAssembly().GetDistricts();
        var match = districts.MatchDistrict("none", "none");
        Assert.Null(match);
    }

    [Fact]
    public void MatchDistrict_Throws_For_Ambiguous_Matches()
    {
        var districts = Assembly.GetExecutingAssembly().GetDistricts();
        Assert.Throws<AmbiguousDistrictMatchException>(() => districts.MatchDistrict("amb", "box"));
    }

    [Theory]
    [InlineData(typeof(Task))]
    [InlineData(typeof(Task<int>))]
    public void IsTask_Returns_True_For_Task_Types(Type t)
    {
        Assert.True(InternalExtensions.IsTask(t));
    }

    [Fact]
    public void IsTask_Returns_False_For_Non_Task_Type()
    {
        Assert.False(InternalExtensions.IsTask(typeof(string)));
    }
}
