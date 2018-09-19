import sys 
import io
#pip install argparse
import argparse
#pip install pdf2jpg

import glob
from pdf2jpg import pdf2jpg 
from os import listdir, rename
from os.path import isfile, join, basename

#pip install google-cloud-vision
from google.cloud import vision
from google.cloud.vision import types

def extractJpegFromPng(inputFolder, outputFolder):
    pdf2jpg.convert_pdf2jpg(inputFolder, outputFolder, pages="ALL")

def removeBookNameFromFile(inputFolder, outputFolder):
    pdfName = basename(inputFolder);

    outputFolder = join(outputFolder,pdfName)

    files = [f for f in listdir(outputFolder) if isfile(join(outputFolder, f))]

    partToRemove = "_" + pdfName
    for file in files:
        newname = file.replace(partToRemove, "")
        rename(join(outputFolder, file), join(outputFolder, newname))

def ocr(inputFolder):
    client = vision.ImageAnnotatorClient()
    
    files = glob.glob("C:\\books\\Rahe Rawan.pdf\\1\\*.jpg")
    for file in files:
        with io.open(file, 'rb') as image_file:
            content = image_file.read()
        image = types.Image(content=content)
        response = client.document_text_detection(image=image)
        text = response.full_text_annotation
        outputPath = file.replace(".jpg", ".txt")
        outfile = open(outputPath, "w")
        outfile.write(text)
        outfile.close()

    


if __name__ == "__main__":
    parser = argparse.ArgumentParser(
        description='Script to import pdf files into inshapardaz book',
        usage='',
        epilog=''
    )

    parser.add_argument('--input', help='pdf file to import', required=True)
    parser.add_argument('--output', help='Name of folder to write output', required=True)

    if len(sys.argv[1:])==0:
        parser.print_help()
        parser.exit()

    args = parser.parse_args()

    extractJpegFromPng(args.input, args.output)
    removeBookNameFromFile(args.input, args.output)