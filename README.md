# Divoom client tool for Pixoo-64

<img src="Images/image.jpg" title="image">

# Install

```
dotnet tool install -g DivoomTool
```

# Usage

## Service

| Command | Pixoo64 | Times Gate | Description |
|:-|:-:|:-:|
| device | ✅ | ✅ | Get LAN device list |
| font | ✅ | ✅ | Get supported font list |


```
divoom device
```

```
divoom font
```

## Channel

| Command | Pixoo64 | Times Gate | Description |
|:-|:-:|:-:|
| current | ✅ | ✅ | Get current channel |
| channel | ✅ | | Set channel type |
| lcd5 list | | ✅ | Get lcd whole list |
| lcd5 info | | ✅ | Get lcd independence information |
| lcd5 channel | | ✅ | Set channel type |
| lcd5 whole | | ✅ | Select whole clock |
| clock type | ✅ | ✅ | Get clock type |
| clock list | ✅ | ✅ | Get clock list |
| clock info | ✅ | | Show clock information |
| clock select | ✅ | ✅ | Select clock |
| cloud select | ✅ | | Select cloud page |
| equalizer select | ✅ | ✅ | Select equalizer |
| cuscom select | ✅ | | Select custom page |
| monitor select | ✅ | ✅ | Select monitor |
| monitor update | ✅ | ✅ | Update monitor |

```
divoom current -h 192.168.100.181
```

```
divoom channel -h 192.168.100.181 -t clock
divoom channel -h 192.168.100.181 -t cloud
divoom channel -h 192.168.100.181 -t equalizer
divoom channel -h 192.168.100.181 -t custom
```

```
divoom lcd5 list -p 1
```

```
divoom lcd5 info
```

```
divoom lcd5 info -d 300000000
```

```
divoom lcd5 channel -h 192.168.100.182 -t w
divoom lcd5 channel -h 192.168.100.182 -t i -l 400000
```

```
divoom lcd5 whole -h 192.168.100.182 -c 581
```

```
divoom clock type
```

```
divoom clock list -t Game -p 1
divoom clock list -t Game -l -p 1
```

```
divoom clock info -h 192.168.100.181
```

```
divoom clock select -h 192.168.100.181 -c 625
divoom clock select -h 192.168.100.182 -c 625 -l 400000 -i 3
```

```
divoom cloud select -h 192.168.100.181 -p 3
```

```
divoom equalizer select -h 192.168.100.181 -p 5
```

```
divoom custom select -h 192.168.100.181 -p 1
```

```
divoom monitor select -h 192.168.100.181
```

```
divoom monitor update -h 192.168.100.181 -d "50 %,10 %,60 C,35 C,40 %,45 C"
```

## Tool

| Command | Pixoo64 | Times Gate | Description |
|:-|:-:|:-:|
| timer | ✅ | ✅ | Timer tool |
| watch | ✅ | ✅ | Stopwatch tool |
| score | ✅ | ✅ | Scoreboard tool |
| noise | ✅ | ✅ | Noise status tool |

```
dovoom timer start -h 192.168.100.181 -s 30
dovoom timer stop -h 192.168.100.181
```

```
dovoom watch start -h 192.168.100.182
dovoom watch stop -h 192.168.100.182
dovoom watch reset -h 192.168.100.182
```

```
dovoom score -h 192.168.100.181 -r 123 -b 456
```

```
dovoom noise start -h 192.168.100.181
dovoom noise stop -h 192.168.100.181
```


----------

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
