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

    filename = '{0:04d}'.format(i)
    im.save(f"{bookname}\{filename}.png") 
    elem.clear()

def clickNext():
    nextButton = driver.find_element_by_css_selector(".left.pull-left.ebookprev")
        
    if not nextButton:
        return False
    else:
        nextButton.click()
        time.sleep(1)
        return True

# makeOutputFolder()

driver = webdriver.Firefox()
driver.set_window_position(0,0)
driver.set_window_size(1000,824)
driver.get(url)

index = 1
while True:
    saveImage(index)
    movedNext = clickNext()
    if not movedNext:
        break 
    index = index + 1

driver.close()