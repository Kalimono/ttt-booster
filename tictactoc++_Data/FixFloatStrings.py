import os
import glob
import csv

dirPaths = glob.glob(os.getcwd() + "*\*")# + "\*\*")
# print([i for i in os.walk(os.getcwd())])
# print(os.getcwd())
filePaths = list()

for (dirpath, dirnames, filenames) in os.walk(os.getcwd()):
    print(filenames)
    break

# for dirPath in dirPaths:
# 	filePaths.append(glob.glob(dirPath + "*\*"))

# for filePath in filePaths:
# 	if len(filePath) != 0:
# 		file = open(filePath[0], "r")
# 		newFile = open(filePath[0]+"FIXED.csv", "w")
# 		csvWriter = csv.writer(newFile, delimiter=";")
# 		for row in file.readlines():
# 			currentRow = [dataPoint.strip("\n").strip(" ") for dataPoint in row.split(";")]

# 			newRow = list()

# 			rowCounter = 0
# 			counter = 0

# 			for dataPoint in currentRow:
# 				newDatapoint = ""
# 				for symbol in dataPoint:
# 					if symbol == ".":
# 						newDatapoint += ","
# 					else:
# 						newDatapoint += symbol
# 					counter += 1
# 				rowCounter += 1
# 				if not rowCounter == len(currentRow):
# 					newDatapoint += " "
# 				newRow.append(newDatapoint)

# 			csvWriter.writerow(newRow)

# 		file.close()
# 		newFile.close()