# Password Generator

Simple password generator for my dev env.

## Execute
Once the password is generated, it is copied to your clipboard.
```
./output/password-generator
-> e#*fUujgNT
```

⚠ On ArchLinux you must have `xsel` to copy the password to your clipboard.

## Settings
```
width: int (= w)
maj: bool
min: bool
symbol: bool (= s)
```

## Example
```
./output/password-generator width:15 s maj min:false
-> #YCQO*MYTHWJATV
```

## Global env.
```
sudo cp ./output/password-generator /usr/local/bin
```
I advise you to create an alias "pg" to use the generator like that
```
pg w:10 maj:false
```

## Compile
```
make build
```