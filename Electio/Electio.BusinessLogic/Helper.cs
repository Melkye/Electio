using Electio.DataAccess.Entities;
using Electio.DataAccess.Enums;

namespace Electio.BusinessLogic;
public static class Helper
{
    // TODO: factor out filtering by year and study component
    public static List<StudyComponent> GetStudyComponentsAvailableToStudyYear(StudyYear studyYear) =>
        studyYear switch
        {
            StudyYear.First => [StudyComponent.SK1],
            StudyYear.Second => [
                StudyComponent.SK2, StudyComponent.SK3, StudyComponent.SK4,
                    StudyComponent.SK5, StudyComponent.SK6, StudyComponent.SK7, StudyComponent.SK8],
            StudyYear.Third => [
                StudyComponent.SK9, StudyComponent.SK10, StudyComponent.SK11,
                    StudyComponent.SK12, StudyComponent.SK13, StudyComponent.SK14],
            _ => throw new ArgumentException($"Student of {studyYear} doesn't elect courses")
        };

    public static StudyYear GetStudyYearByStudyComponent(StudyComponent studyComponent) =>
    studyComponent switch
    {
        StudyComponent.SK1 => StudyYear.First,

        StudyComponent.SK2 => StudyYear.Second,
        StudyComponent.SK3 => StudyYear.Second,
        StudyComponent.SK4 => StudyYear.Second,
        StudyComponent.SK5 => StudyYear.Second,
        StudyComponent.SK6 => StudyYear.Second,
        StudyComponent.SK7 => StudyYear.Second,
        StudyComponent.SK8 => StudyYear.Second,

        StudyComponent.SK9 => StudyYear.Third,
        StudyComponent.SK10 => StudyYear.Third,
        StudyComponent.SK11 => StudyYear.Third,
        StudyComponent.SK12 => StudyYear.Third,
        StudyComponent.SK13 => StudyYear.Third,
        StudyComponent.SK14 => StudyYear.Third,

        _ => throw new ArgumentException($"Error occured")
    };
}
