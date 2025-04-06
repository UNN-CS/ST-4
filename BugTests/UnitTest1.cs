
namespace BugTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestAssignFromOpen()
		{
			var bug = new Bug(Bug.State.Open);
			bug.Assign();
			Assert.AreEqual(Bug.State.Assigned, bug.getState());
		}

		[TestMethod]
		public void TestRejectFromOpen()
		{
			var bug = new Bug(Bug.State.Open);
			bug.Reject();
			Assert.AreEqual(Bug.State.Rejected, bug.getState());
		}

		[TestMethod]
		public void TestDeferFromAssigned()
		{
			var bug = new Bug(Bug.State.Assigned);
			bug.Defer();
			Assert.AreEqual(Bug.State.Defered, bug.getState());
		}

		[TestMethod]
		public void TestVerifyFromAssigned()
		{
			var bug = new Bug(Bug.State.Assigned);
			bug.Verify();
			Assert.AreEqual(Bug.State.Verified, bug.getState());
		}

		[TestMethod]
		public void TestCloseFromAssigned()
		{
			var bug = new Bug(Bug.State.Assigned);
			bug.Close();
			Assert.AreEqual(Bug.State.Closed, bug.getState());
		}

		[TestMethod]
		public void TestCloseFromVerified()
		{
			var bug = new Bug(Bug.State.Verified);
			bug.Close();
			Assert.AreEqual(Bug.State.Closed, bug.getState());
		}

		[TestMethod]
		public void TestAssignFromDefered()
		{
			var bug = new Bug(Bug.State.Defered);
			bug.Assign();
			Assert.AreEqual(Bug.State.Assigned, bug.getState());
		}

		[TestMethod]
		public void TestReopenFromClosed()
		{
			var bug = new Bug(Bug.State.Closed);
			bug.Reopen();
			Assert.AreEqual(Bug.State.Reopened, bug.getState());
		}

		[TestMethod]
		public void TestAssignFromClosed()
		{
			var bug = new Bug(Bug.State.Closed);
			bug.Assign();
			Assert.AreEqual(Bug.State.Assigned, bug.getState());
		}

		[TestMethod]
		public void TestAssignFromReopened()
		{
			var bug = new Bug(Bug.State.Reopened);
			bug.Assign();
			Assert.AreEqual(Bug.State.Assigned, bug.getState());
		}

		[TestMethod]
		public void TestRejectFromReopened()
		{
			var bug = new Bug(Bug.State.Reopened);
			bug.Reject();
			Assert.AreEqual(Bug.State.Rejected, bug.getState());
		}

		[TestMethod]
		public void TestReopenFromRejected()
		{
			var bug = new Bug(Bug.State.Rejected);
			bug.Reopen();
			Assert.AreEqual(Bug.State.Reopened, bug.getState());
		}

		[TestMethod]
		public void TestIgnoreAssignFromAssigned()
		{
			var bug = new Bug(Bug.State.Assigned);
			bug.Assign();
			Assert.AreEqual(Bug.State.Assigned, bug.getState());
		}

		[TestMethod]
		public void TestInvalidDeferFromOpenThrows()
		{
			var bug = new Bug(Bug.State.Open);
			Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
		}

		[TestMethod]
		public void TestInvalidCloseFromOpenThrows()
		{
			var bug = new Bug(Bug.State.Open);
			Assert.ThrowsException<InvalidOperationException>(() => bug.Close());
		}

		[TestMethod]
		public void TestInvalidVerifyFromOpenThrows()
		{
			var bug = new Bug(Bug.State.Open);
			Assert.ThrowsException<InvalidOperationException>(() => bug.Verify());
		}

		[TestMethod]
		public void TestInvalidAssignFromVerifiedThrows()
		{
			var bug = new Bug(Bug.State.Verified);
			Assert.ThrowsException<InvalidOperationException>(() => bug.Assign());
		}

		[TestMethod]
		public void TestInvalidDeferFromVerifiedThrows()
		{
			var bug = new Bug(Bug.State.Verified);
			Assert.ThrowsException<InvalidOperationException>(() => bug.Defer());
		}

		[TestMethod]
		public void TestInvalidReopenFromOpenThrows()
		{
			var bug = new Bug(Bug.State.Open);
			Assert.ThrowsException<InvalidOperationException>(() => bug.Reopen());
		}

		[TestMethod]
		public void TestFullTransitionScenario()
		{
			var bug = new Bug(Bug.State.Open);
			bug.Assign();
			bug.Verify();
			bug.Close();
			bug.Reopen();
			bug.Assign();
			bug.Defer();
			bug.Assign();
			bug.Reject();
			bug.Reopen();
			bug.Assign();
			Assert.AreEqual(Bug.State.Assigned, bug.getState());
		}
	}
}
