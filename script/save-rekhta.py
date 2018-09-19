from selenium import webdriver
from selenium.webdriver.common.keys import Keys

from PIL import Image
from io import BytesIO

url = "https://www.rekhta.org/ebooks/is-basti-ke-ek-kooche-mein-ibn-e-insha-ebooks-1"
pageCount = 199

def saveImage(i):
    elem = driver.find_elements_by_tag_name("canvas")
    element = elem[0]
    location = element.location
    size = element.size

    png = driver.get_screenshot_as_png()

    im = Image.open(BytesIO(png))

    left = location['x']
    top = location['y']
    right = location['x'] + size['width']
    bottom = location['y'] + size['height']


    im = im.crop((left, top, right, bottom))

    im.save(f"{i}.png") 
    elem.clear()

def clickNext():
    nextButton = driver.find_element_by_css_selector(".left.pull-left.ebookprev")
    nextButton.click()

driver = webdriver.Firefox()
driver.set_window_position(0,0)
driver.set_window_size(970,824)
driver.get(url)

index = 1
while index <= pageCount:
    saveImage(index)
    clickNext()
    index = index + 1

driver.close()