package com.example.h4selfie;

/**
 * A test class to test how javadoc works :P
 * @author h4ck3 rm4n
 * @deprecated
 */
public class Image {
    String name;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    /**
     * @param name The filename of the image, not full path of file on the file system
     */
    public Image(String name) {
        this.name = name;
    }
}
