# Divoom client tool for Pixoo-64

# Install

```
dotnet tool install -g DivoomTool
```

# Usage

## Service

### Get LAN device list

```
divoom device
```

### Get supported font list

```
divoom font
```

### Get dial type

```
divoom dial type
```

### Get dial list

(TODO)



## Channel

### Get current channel

```
divoom cunnel -h 192.168.1.101
```

### Clock channel

```
divoom clock -h 192.168.1.101
```

```
divoom clock info -h 192.168.1.101
```

```
divoom clock select -h 192.168.1.101 -i 182
```

### Cloud channel

```
divoom cloud -h 192.168.1.101
```

```
divoom cloud select -h 192.168.1.101 -p 1
```

### Equalizer channel

```
divoom equalizer -h 192.168.1.101
```

```
divoom equalizer select -h 192.168.1.101 -p 1
```

### Custom channel

```
divoom custom -h 192.168.1.101
```

```
divoom custom select -h 192.168.1.101 -p 1
```

### Monitor

```
divoom monitor -h 192.168.1.101
```

```
divoom monitor yodate -h 192.168.1.101 -d "50 %,10 %,60 C,35 C,40 %,45 C"
```



## Tool

### Timer tool

```
divoom timer start -h 192.168.1.101 -s 30
```

```
divoom timer stop -h 192.168.1.101
```

### Stopwatch tool

```
divoom watch start -h 192.168.1.101
```

```
divoom watch stop -h 192.168.1.101
```

```
divoom watch reset -h 192.168.1.101
```

### Scoreboard tool

```
divoom score -h 192.168.1.101 -b 123 -r 456
```

### Noise tool

```
divoom noise start -h 192.168.1.101
```

```
divoom noise stop -h 192.168.1.101
```



## Image/Text

### Reset image id

```
divoom image reset -h 192.168.1.101
```

### Get image id

```
divoom image id -h 192.168.1.101
```

### Draw image

```
divoom image draw -h 192.168.1.101 -f cat.png
```

### Fill image

```
divoom image fill -h 192.168.1.101 -c #000000
```

### List upload images

(TODO)

### List like images

(TODO)

### Draw remote image

(TODO)

### Draw text

(TODO)

### Display item list

```
divoom display -h 192.168.1.101 -f items.json
```

```json
{
    ...
}
```

### Play gif

(TODO)



## Device

### Get/Set device time

(TODO)

### Get device weather

```
divoom weather -h 192.168.1.101
```

### Play buzzer

```
divoom buzzer -h 192.168.1.101 -a 500 -f 500 -t 3000
```

### Screen switch

(TODO)

### Set brightness

(TODO)

### Set rotation

(TODO)

### Set mirror mode

(TODO)

### Set highlight mode

(TODO)

### Set white balance

(TODO)



## Config

### Get all config

```
divoom config -h 192.168.1.101
```

### Set area

```
divoom area -h 192.168.1.101 --lon 138.608 --lat 35.616
```

### Set timezone

```
divoom timezone -h 192.168.1.101 -z GMT+9
```

### Set temperature mode

```
divoom temperature -h 192.168.1.101 -m c
```

### Set hour mode

```
divoom hour -h 192.168.1.101 -m 24
```



## System

### Reboot

```
divoom reboot -h 192.168.1.101
```

# Reference

https://docin.divoom-gz.com/web/#/5/23
