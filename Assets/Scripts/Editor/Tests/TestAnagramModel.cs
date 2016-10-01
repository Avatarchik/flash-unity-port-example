using NUnit.Framework/*<Test>*/;

namespace Finegamedesign.Anagram
{
	public sealed class TestAnagramModel
	{
		private static string DescribeTrialCycle(AnagramModel model)
		{
			return "trial count " + model.trialCount 
				+ " in trial period " + model.trialPeriod;
		}

		[Test]
		public void IsTrialCycleNow()
		{
			AnagramModel model = new AnagramModel();
			model.progress.level = model.tutorLevel;
			Assert.AreEqual(false, model.IsTutor(), "tutor");
			model.trialPeriod = 10;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 5;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 9;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 10;
			Assert.AreEqual(true, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 11;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 19;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 20;
			Assert.AreEqual(true, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 21;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
		}

		[Test]
		public void IsTrialCycleNowTutorial()
		{
			AnagramModel model = new AnagramModel();
			Assert.AreEqual(true, model.IsTutor(), "tutor");
			model.trialPeriod = 5;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 4;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 5;
			Assert.AreEqual(true, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 9;
			Assert.AreEqual(false, model.IsTrialCycleNow(), DescribeTrialCycle(model));
			model.trialCount = 10;
			Assert.AreEqual(true, model.IsTrialCycleNow(), DescribeTrialCycle(model));
		}

		[Test]
		public void UpdateTrialCycleCheckpoint()
		{
			AnagramModel model = new AnagramModel();
			model.progress.level = model.tutorLevel;
			Assert.AreEqual(false, model.IsTutor(), "tutor");
			model.trialPeriod = 3;
			Assert.AreEqual(false, model.UpdateTrialCycleCheckpoint(), DescribeTrialCycle(model));
			Assert.AreEqual(false, model.isContinueVisible, "Is continue visible");
			model.trialCount = 0;
			Assert.AreEqual(false, model.UpdateTrialCycleCheckpoint(), DescribeTrialCycle(model));
			Assert.AreEqual(false, model.isContinueVisible, "Is continue visible");
			model.trialCount = 1;
			Assert.AreEqual(false, model.UpdateTrialCycleCheckpoint(), DescribeTrialCycle(model));
			Assert.AreEqual(false, model.isContinueVisible, "Is continue visible");
			model.trialCount = 2;
			Assert.AreEqual(false, model.UpdateTrialCycleCheckpoint(), DescribeTrialCycle(model));
			Assert.AreEqual(false, model.isContinueVisible, "Is continue visible");
			model.trialCount = 3;
			Assert.AreEqual(true, model.UpdateTrialCycleCheckpoint(), DescribeTrialCycle(model));
			Assert.AreEqual(true, model.isContinueVisible, "Is continue visible");
			model.isContinueVisible = false;
			model.trialCount = 4;
			Assert.AreEqual(false, model.UpdateTrialCycleCheckpoint(), DescribeTrialCycle(model));
			Assert.AreEqual(false, model.isContinueVisible, "Is continue visible");
		}
	}
}