using BugPro;

namespace BugTests
{
    [TestClass]
    public sealed class Test1
    {
        private Bug _bug;

        [TestInitialize]
        public void Setup()
        {
            _bug = new Bug("TEST-001");
        }

        // Проверка 1: Из состояния "Новый" можно перейти в "Разбор дефектов"
        [TestMethod]
        public void Test01_FromNewState_TransitionToTriage_ShouldSucceed()
        {
            Assert.AreEqual(BugState.New, _bug.State);
            _bug.Fire(BugTrigger.AssignToTriage);
            Assert.AreEqual(BugState.Triage, _bug.State);
        }

        // Проверка 2: Из состояния "Разбор дефектов" можно начать исправление
        [TestMethod]
        public void Test02_FromTriageState_StartFix_ShouldMoveToFixing()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.StartFix);
            Assert.AreEqual(BugState.Fixing, _bug.State);
        }

        // Проверка 3: Из состояния "Исправление" можно передать баг тестировщику
        [TestMethod]
        public void Test03_FromFixingState_SendToTester_ShouldMoveToTesting()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.StartFix);
            _bug.Fire(BugTrigger.ToTester);
            Assert.AreEqual(BugState.Testing, _bug.State);
        }

        // Проверка 4: Из состояния "Тестировщик" при ответе "Проблема не воспроизводится" баг закрывается
        [TestMethod]
        public void Test04_FromTestingState_ProblemNo_ShouldMoveToClosing()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.StartFix);
            _bug.Fire(BugTrigger.ToTester);
            _bug.Fire(BugTrigger.ProblemNo);
            Assert.AreEqual(BugState.Closing, _bug.State);
        }

        // Проверка 5: Из состояния "Разбор дефектов" можно пометить баг как не являющийся дефектом
        [TestMethod]
        public void Test05_FromTriageState_MarkNotADefect_ShouldMoveToNotADefect()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.NotADefect);
            Assert.AreEqual(BugState.NotADefect, _bug.State);
        }

        // Проверка 6: Из состояния "Разбор дефектов" можно отклонить исправление
        [TestMethod]
        public void Test06_FromTriageState_MarkWontFix_ShouldMoveToWontFix()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.WontFix);
            Assert.AreEqual(BugState.WontFix, _bug.State);
        }

        // Проверка 7: Из состояния "Разбор дефектов" можно пометить баг как дубликат
        [TestMethod]
        public void Test07_FromTriageState_MarkDuplicate_ShouldMoveToDuplicate()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.Duplicate);
            Assert.AreEqual(BugState.Duplicate, _bug.State);
        }

        // Проверка 8: Из состояния "Разбор дефектов" можно указать, что проблема не воспроизводится
        [TestMethod]
        public void Test08_FromTriageState_MarkCannotReproduce_ShouldMoveToCannotReproduce()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.CannotReproduce);
            Assert.AreEqual(BugState.CannotReproduce, _bug.State);
        }

        // Проверка 9: В состоянии "Разбор дефектов" триггер "Нет времени" не меняет состояние
        [TestMethod]
        public void Test09_InTriageState_TriggerNoTime_StateRemainsUnchanged()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            BugState stateBefore = _bug.State;
            _bug.Fire(BugTrigger.NoTime);
            Assert.AreEqual(stateBefore, _bug.State);
        }

        // Проверка 10: В состоянии "Разбор дефектов" триггер "Нужно больше информации" не меняет состояние
        [TestMethod]
        public void Test10_InTriageState_TriggerNeedMoreInfo_StateRemainsUnchanged()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            BugState stateBefore = _bug.State;
            _bug.Fire(BugTrigger.NeedMoreInfo);
            Assert.AreEqual(stateBefore, _bug.State);
        }

        // Проверка 11: В состоянии "Разбор дефектов" триггер "Нужно отдельное решение" не меняет состояние
        [TestMethod]
        public void Test11_InTriageState_TriggerSeparateSolution_StateRemainsUnchanged()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            BugState stateBefore = _bug.State;
            _bug.Fire(BugTrigger.SeparateSolution);
            Assert.AreEqual(stateBefore, _bug.State);
        }

        // Проверка 12: В состоянии "Разбор дефектов" триггер "Проблема другого продукта" не меняет состояние
        [TestMethod]
        public void Test12_InTriageState_TriggerOtherProduct_StateRemainsUnchanged()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            BugState stateBefore = _bug.State;
            _bug.Fire(BugTrigger.OtherProduct);
            Assert.AreEqual(stateBefore, _bug.State);
        }

        // Проверка 13: Из состояния "Исправление" можно передать баг продуктовой команде
        [TestMethod]
        public void Test13_FromFixingState_SendToProductTeam_ShouldMoveToProductTeam()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.StartFix);
            _bug.Fire(BugTrigger.ToProductTeam);
            Assert.AreEqual(BugState.ProductTeam, _bug.State);
        }

        // Проверка 14: Из состояния "Исправление" можно передать баг разработчику
        [TestMethod]
        public void Test14_FromFixingState_SendToDeveloper_ShouldMoveToDeveloper()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.StartFix);
            _bug.Fire(BugTrigger.ToDeveloper);
            Assert.AreEqual(BugState.Developer, _bug.State);
        }

        // Проверка 15: В состоянии "Продуктовая команда" при ответе "Проблема не воспроизводится" баг закрывается
        [TestMethod]
        public void Test15_InProductTeamState_ProblemNo_ShouldMoveToClosing()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.StartFix);
            _bug.Fire(BugTrigger.ToProductTeam);
            _bug.Fire(BugTrigger.ProblemNo);
            Assert.AreEqual(BugState.Closing, _bug.State);
        }

        // Проверка 16: В состоянии "Разработчик" при подтверждении проблемы переходим к поиску решения
        [TestMethod]
        public void Test16_InDeveloperState_ProblemYes_ShouldMoveToProblemSolving()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.StartFix);
            _bug.Fire(BugTrigger.ToDeveloper);
            _bug.Fire(BugTrigger.ProblemYes);
            Assert.AreEqual(BugState.ProblemSolving, _bug.State);
        }

        // Проверка 17: Из состояния "Проблема решения?" при отрицательной проверке переходим к повторной проверке
        [TestMethod]
        public void Test17_FromProblemSolvingState_OkNo_ShouldMoveToOkCheck()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.StartFix);
            _bug.Fire(BugTrigger.ToTester);
            _bug.Fire(BugTrigger.ProblemYes);
            _bug.Fire(BugTrigger.OkNo);
            Assert.AreEqual(BugState.OkCheck, _bug.State);
        }

        // Проверка 18: В состоянии "OK?" повторный отрицательный ответ возвращает баг в разбор дефектов
        [TestMethod]
        public void Test18_InOkCheckState_SecondOkNo_ShouldMoveToTriage()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.StartFix);
            _bug.Fire(BugTrigger.ToTester);
            _bug.Fire(BugTrigger.ProblemYes);
            _bug.Fire(BugTrigger.OkNo);
            _bug.Fire(BugTrigger.OkNo);
            Assert.AreEqual(BugState.Triage, _bug.State);
        }

        // Проверка 19: В состоянии "OK?" при положительном ответе баг закрывается
        [TestMethod]
        public void Test19_InOkCheckState_OkYes_ShouldMoveToClosing()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.StartFix);
            _bug.Fire(BugTrigger.ToTester);
            _bug.Fire(BugTrigger.ProblemYes);
            _bug.Fire(BugTrigger.OkNo);
            _bug.Fire(BugTrigger.OkYes);
            Assert.AreEqual(BugState.Closing, _bug.State);
        }

        // Проверка 20: Из состояния "Не дефект" можно переоткрыть баг
        [TestMethod]
        public void Test20_FromNotADefectState_Reopen_ShouldMoveToReopened()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.NotADefect);
            _bug.Fire(BugTrigger.Reopen);
            Assert.AreEqual(BugState.Reopened, _bug.State);
        }

        // Проверка 21: Из переоткрытого состояния можно снова отправить баг в разбор дефектов
        [TestMethod]
        public void Test21_FromReopenedState_AssignToTriage_ShouldMoveToTriage()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.NotADefect);
            _bug.Fire(BugTrigger.Reopen);
            _bug.Fire(BugTrigger.AssignToTriage);
            Assert.AreEqual(BugState.Triage, _bug.State);
        }

        // Проверка 22: Новый баг не должен переходить в состояние "Исправление" без предварительного разбора
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test22_NewState_StartFixWithoutTriage_ShouldThrowException()
        {
            _bug.Fire(BugTrigger.StartFix);
        }

        // Проверка 23: Из состояния "Разбор дефектов" нельзя передать баг тестировщику без этапа исправления
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test23_TriageState_SendToTesterWithoutFixing_ShouldThrowException()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.ToTester);
        }

        // Проверка 24: Закрытый баг нельзя переоткрыть более одного раза
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test24_ClosedBug_ReopenTwice_ShouldThrowException()
        {
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.NotADefect);
            _bug.Fire(BugTrigger.Reopen);
            _bug.Fire(BugTrigger.AssignToTriage);
            _bug.Fire(BugTrigger.Reopen);
        }
    }
}
