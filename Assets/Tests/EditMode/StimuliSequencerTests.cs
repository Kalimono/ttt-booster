// using System.Collections;
// using System.Collections.Generic;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;
// using System.Linq;

// namespace Tests
// {
//     public class StimuliSequencerTests
//     {
//         [Test]
//         public void CreateStimuliSequenceSetsSequenceCountToTheSpecifiedCount() {   
//             StimuliSequencer.CreateStimuliSequence();
//             Assert.AreEqual(StimuliSequencer.sequenceLength, StimuliSequencer.stimuliSequence.Count);
//         }

//         // [Test]
//         // public void GetRandomCorrectPositionReturnIsCorrectType() {   
//         //     var randPos = StimuliSequencer.GetRandomCorrectPosition();
//         //     Assert.True(randPos is int);
//         // }

//         [Test]
//         public void GetRandomCorrectPositionReturnIsAllowedNum() {   
//             List<int> allowedNums = new List<int> {0, 2, 6, 8};
//             int randPos = StimuliSequencer.GetRandomCorrectPosition();
//             Assert.True(allowedNums.Contains(randPos));
//         }

//         [Test]
//         public void CreateSquarePositionSequenceCountToTheSpecifiedCount() {   
//             StimuliSequencer.CreateSquarePositionSequence();
//             Assert.AreEqual(StimuliSequencer.sequenceLength, StimuliSequencer.squarePositionSequence.Count);
//         }

//         [Test]
//         public void CreateSquarePositionSequenceContainsOnlyAllowedNums() {   
//             List<int> allowedNums = new List<int> {0, 1, 2, 3};
//             foreach(int num in StimuliSequencer.squarePositionSequence) {
//                 Assert.True(allowedNums.Contains(num));
//             }
//         }

//         [Test]
//         public void CreateSquarePositionSequenceIsEquallyDistributed() {   
//             StimuliSequencer.CreateSquarePositionSequence();
//             var sortCount = StimuliSequencer.squarePositionSequence.GroupBy(r => r).Select(group => new{Value = group.Key,Count = group.Count()});
//             foreach (var number in sortCount) { 
//                 Assert.True(number.Count == StimuliSequencer.squarePositionSequence.Count/sortCount.Count()); 
//             }
//         }

//         [Test]
//         public void CreateTargetSequenceCountToTheSpecifiedCount() {  
//             StimuliSequencer.CreateTargetSequence(); 
//             Assert.AreEqual(StimuliSequencer.sequenceLength, StimuliSequencer.targetSequence.Count());
//         }

//         [Test]
//         public void CreateTargetSequenceContainsOnlyAllowedNums() {   
//             StimuliSequencer.CreateTargetSequence();
//             List<int> allowedNums = new List<int> {0, 1};
//             foreach (int target in StimuliSequencer.targetSequence) {
//                 Assert.True(allowedNums.Contains(target));
//             }
//         }

//         [Test]
//         public void CreateTargetSequenceIsEquallyDistributed() {   
//             StimuliSequencer.CreateTargetSequence();
//             var sortedAndCounted = StimuliSequencer.targetSequence.GroupBy(r => r).Select(group => new{Value = group.Key,Count = group.Count()});
//             foreach (var number in sortedAndCounted) {
//                 Assert.True(number.Count == StimuliSequencer.targetSequence.Count/sortedAndCounted.Count());
//             }
//         }

//         // [Test]
//         // public void GetStimuliIndexReturnIsCorrectType() {   
//         //     var randNum = StimuliSequencer.GetStimuliIndex();
//         //     Assert.True(randNum is int);
//         // }

//         // [Test]
//         // public void GetTargetNumReturnIsCorrectType() {   
//         //     var randNum = StimuliSequencer.GetTargetNum();
//         //     Assert.True(randNum is int);
//         // }

//         [Test]
//         public void AddListToListCountToThePreviousCount() {   
//             List<int> listToBeAddedTo = new List<int>();
//             List<int> listToBeAddedFrom = new List<int>{1, 3, 3, 7};

//             StimuliSequencer.AddListToList(listToBeAddedTo, listToBeAddedFrom);

//             Assert.AreEqual(listToBeAddedTo.Count, listToBeAddedFrom.Count);
//         }

//         [Test]
//         public void RandomizeListCountToThePreviousCount() { 
//             List<int> listToBeRandomized = new List<int>{1, 3, 3, 7};
//             List<int> randomizedList = StimuliSequencer.RandomizeList(listToBeRandomized); 
//             Assert.AreEqual(randomizedList.Count, listToBeRandomized.Count);
//         }

//         [Test]
//         public void RandomizeListIsEquallyDistributed() {   
//             List<int> listToBeRandomized = new List<int>{1, 3, 3, 7};
//             List<int> randomizedList = StimuliSequencer.RandomizeList(listToBeRandomized); 
//             listToBeRandomized.Sort();
//             randomizedList.Sort();

//             for(int i = 0; i<listToBeRandomized.Count; i++) {
//                 Assert.True(listToBeRandomized[i] == randomizedList[i]);
//             }
//         }
//     }
// }
