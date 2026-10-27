using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("ToRefactor")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Сравнение с помощью Fluent Assertions
        // Сравнение объектов по свойствам, игнорирует Id и Id родителя
        // Метод сравнивает все свойства, кроме тех, что мы исключили
        // В отличие от AreEqual в методе CheckCurrentTsar_WithCustomEquality, который сравнивает только те свойства, которые мы указали в коде, если в person добавятся свойства, то нужно будет переписывать тест
        // Ещё метод BeEquivalentTo описан в документации, можно посмотреть его возможности и поведение. А для AreEqual скорее всего документации нет и нужно разбираться в коде, либо самому писать документацию
        // Также AreEqual возвращает true/false, а BeEquivalentTo выбрасывает исключение, если объекты не равны. Мы можем увидеть что именно не совпало
        // И напоследок AreEqual рекурсивно проверяет родителей батя-дед-прадед и тд. по идее для проверки текущего царя такая длинная цепочка проверок не нужна
        actualTsar.Should().BeEquivalentTo(expectedTsar, options => options
            .Excluding(person => person.Id)
            .Excluding(person => person.Parent!.Id));
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
