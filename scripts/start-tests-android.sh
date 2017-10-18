#!/bin/sh
ADB_PATH=/media/data/android-sdk-linux/platform-tools/
ADB=$ADB_PATH/adb
$ADB uninstall ClientTest.ClientTest
$ADB install $1
$ADB shell am start -a "android.intent.action.MAIN" -c "android.intent.category.LAUNCHER" -n "ClientTest.ClientTest/md566a89c39e384e18e9078bcff3f5a611c.MainActivity"
$ADB logcat
