﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCop.Aspects.Tests.ActionWith1ArgumentAspect.Subjects;
using System;
using System.Collections.Generic;

namespace NCop.Aspects.Tests
{
    [TestClass]
    public class ActionWith1ArgumentAspectTest : AbstractAspectTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ActionWith1Argument_AnnotatedWithOnMethodBoundaryAspect_ReturnsTheCorrectSequenceOfAdvices() {
            var instance = container.Resolve<IActionWith1ArgumentComposite>();
            var list = new List<AspectJoinPoints>();

            instance.OnMethodBoundaryAspect(list);

            CollectionAssert.AreEqual(list, new OnMethodBoundaryAspectOrderedJoinPoints());
            CollectionAssert.DoesNotContain(list, AspectJoinPoints.OnException);
        }

        [TestMethod]
        public void ActionWith1Argument_AnnotatedWithInterceptionAspect_ReturnsTheCorrectSequenceOfAdvices() {
            var instance = container.Resolve<IActionWith1ArgumentComposite>();
            var list = new List<AspectJoinPoints>();

            instance.InterceptionAspect(list);
            CollectionAssert.AreEqual(list, new InterceptionAspectOrderedJoinPoints());
        }

        [TestMethod]
        public void ActionWith1Argument_AnnotatedWithMultipleInterceptionAspects_ReturnsTheCorrectSequenceOfAdvices() {
            var instance = container.Resolve<IActionWith1ArgumentComposite>();
            var list = new List<AspectJoinPoints>();

            instance.MultipleInterceptionAspects(list);
            CollectionAssert.AreEqual(list, new MultipleInterceptionAspectOrderedJoinPoints());
        }

        [TestMethod]
        public void ActionWith1Argument_AnnotatedWithMultipleOnMethodBoundaryAspects_ReturnsTheCorrectSequenceOfAdvices() {
            var instance = container.Resolve<IActionWith1ArgumentComposite>();
            var list = new List<AspectJoinPoints>();

            instance.MultipleOnMethodBoundaryAspects(list);

            CollectionAssert.AreEqual(list, new MultipleOnMethodBoundaryAspectOrderedJoinPoints());
            CollectionAssert.DoesNotContain(list, AspectJoinPoints.OnException);
        }

        [TestMethod]
        public void ActionWith1Argument_AnnotatedWithAllAspectsStartingWithInterceptionAspect_ReturnsTheCorrectSequenceOfAdvices() {
            var instance = container.Resolve<IActionWith1ArgumentComposite>();
            var list = new List<AspectJoinPoints>();

            instance.AllAspectsStartingWithInterception(list);
            CollectionAssert.AreEqual(list, new AllAspectOrderedJoinPointsStartingWithInterceptionAspect());
        }

        [TestMethod]
        public void ActionWith1Argument_AnnotatedWithAllAspectsStartingWithOnMethodBoundaryAspect_ReturnsTheCorrectSequenceOfAdvices() {
            var instance = container.Resolve<IActionWith1ArgumentComposite>();
            var list = new List<AspectJoinPoints>();

            instance.AllAspectsStartingWithOnMethodBoundary(list);

            CollectionAssert.AreEqual(list, new AllAspectOrderedJoinPointsStartingWithOnMethodBoundaryAspect());
            CollectionAssert.DoesNotContain(list, AspectJoinPoints.OnException);
        }

        [TestMethod]
        public void ActionWith1Argument_AnnotatedWithAlternateAspectsStartingWithInterceptionAspect_ReturnsTheCorrectSequenceOfAdvices() {
            var instance = container.Resolve<IActionWith1ArgumentComposite>();
            var list = new List<AspectJoinPoints>();

            instance.AlternatelAspectsStartingWithInterception(list);
            CollectionAssert.AreEqual(list, new AlternateAspectOrderedJoinPointsStartingWithInterceptionAspect());
        }

        [TestMethod]
        public void ActionWith1Argument_AnnotatedWithAlternateAspectsStartingWithOnMethodBoundaryAspect_ReturnsTheCorrectSequenceOfAdvices() {
            var instance = container.Resolve<IActionWith1ArgumentComposite>();
            var list = new List<AspectJoinPoints>();

            instance.AlternateAspectsStartingWithOnMethodBoundary(list);

            CollectionAssert.AreEqual(list, new AlternateAspectOrderedJoinPointsStartingWithOnMethodBoundaryAspect());
            CollectionAssert.DoesNotContain(list, AspectJoinPoints.OnException);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ActionWith1Argument_AnnotatedWithOnMethodBoundaryAspectThatRaisesAnExceptionInMethodInvocationWithDefaultFlowBehaviour_ThrowsException() {
            var instance = container.Resolve<IActionWith1ArgumentComposite>();
            var list = new List<AspectJoinPoints>();

            instance.OnMethodBoundaryAspectThatRaiseAnExceptionInMethodImpl(list);
        }

        [TestMethod]
        public void ActionWith1Argument_AnnotatedWithOnMethodBoundaryAspectThatRaisesAnExceptionInMethodInvocationWithContinueFlowBehaviour_OmitsTheOnSuccessAdvice() {
            var instance = container.Resolve<IActionWith1ArgumentComposite>();
            var list = new List<AspectJoinPoints>();

            instance.OnMethodBoundaryAspectThatRaiseAnExceptionInMethodImplDecoratedWithContinueFlowBehaviourAspect(list);
            CollectionAssert.AreEqual(list, new WithExceptionFlowBehaviourContinueOnMethodBoundaryAspectOrderedJoinPoints());
        }

        [TestMethod]
        public void ActionWith1Argument_AnnotatedWithATryFinallyOnMethodBoundaryAspectThatRaisesAnExceptionInMethodInvocation_OmitsTheOnSuccessAdviceAndReturnsTheCorrectSequenceOfAdvices() {
            var instance = container.Resolve<IActionWith1ArgumentComposite>();
            var list = new List<AspectJoinPoints>();

            try {
                instance.TryfinallyOnMethodBoundaryAspectThatRaiseAnExceptionInMethodImpl(list);
            }
            catch (Exception) {
                CollectionAssert.AreEqual(list, new TryFinallyWithExceptionOnMethodBoundaryAspectOrderedJoinPoints());
            }
        }

        [TestMethod]
        public void ActionWith1Argument_OnMethodBoundaryAspectThatRaisesAnExceptionInMethodInvocationWithoutTryFinally_OmitsTheOnSuccessAdviceAndReturnsTheCorrectSequenceOfAdvices() {
            var instance = container.Resolve<IActionWith1ArgumentComposite>();
            var list = new List<AspectJoinPoints>();

            try {
                instance.OnMethodBoundaryAspectThatRaiseAnExceptionInMethodImplWithoutTryFinally(list);
            }
            catch (Exception) {
                CollectionAssert.AreEqual(list, new OnMethodBoundaryAspectWithExceptionAndWithoutTryFinallyOrderedJoinPoints());
            }
        }

        [TestMethod]
        public void ActionWith1Argument_OnMethodBoundaryAspectWithOnlyOnEntryAdvice_ReturnsTheCorrectSequenceOfAdvices() {
            var instance = container.Resolve<IActionWith1ArgumentComposite>();
            var list = new List<AspectJoinPoints>();

            instance.OnMethodBoundaryAspectWithOnlyOnEntryAdvide(list);

            CollectionAssert.AreEqual(list, new OnMethodBoundaryAspectWithOnlyOnEntryAdviceOrderedJoinPoints());
        }

        [TestMethod]
        public void ActionWith1Argument_AnnotatedWithAllAspectsStartingFromInterceptionAspectThatCallsTheInvokeMethodOfTheArgs_ReturnsTheInMethodAdviceAndIgnoresAllOtherAspects() {
            var instance = container.Resolve<IActionWith1ArgumentComposite>();
            var list = new List<AspectJoinPoints>();

            instance.InterceptionAspectUsingInvoke(list);

            CollectionAssert.AreEqual(list, new InterceptionAspectUsingInvokeOrderedJoinPoints());
        }
    }
}