#pip install selenium
from selenium import webdriver
from selenium.webdriver.common.keys import Keys

#pip install Pillow
from PIL import Image
from io import BytesIO
import time
import os

url = "https://www.rekhta.org/ebooks/bajang-aamad-col-mohammad-khan-ebooks-1"
bookname = "BajangAmad"

def makeOutputFolder():
    filePath = f"{os.path.dirname(os.path.realpath(__file__))}\{bookname}"
    if not os.path.exists(os.path.dirname(filePath)):
        try:
            os.makedirs(os.path.dirname(filePath))
        except OSError as exc: # Guard against race condition
            if exc.errno != errno.EEXIST:
                raise

def saveImage(i):
    elem = driver.find_elements_by_id("actualRenderingDiv")
    element = elem[0]
    location = element.location
    size = element.size

    png = driver.get_screenshot_as_png()

    im = Image.open(BytesIO(png))

    left = location['x']
    top = location['y'] + 30
    right = location['x'] + size['width']
    bottom = location['y'] + size['height']


    im = im.crop((left, top, right, bottom))

    im.save(f"{bookname}\{i}.png") 
    elem.clear()

def clickNext():
    nextButton = driver.find_element_by_css_selector(".left.pull-left.ebookprev")
    nextButton.click()
    time.sleep(1)

# makeOutputFolder()

driver = webdriver.Firefox()
driver.set_window_position(0,0)
driver.set_window_size(1000,824)
driver.get(url)

index = 1
while True:
    saveImage(index)
    clickNext()
    index = index + 1

driver.close()