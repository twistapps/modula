# [0.7.0](https://github.com/twistapps/modula/compare/0.6.1...0.7.0) (2022-04-21)


### Bug Fixes

* GetModule now returns null instead of throwing errors ([57163d9](https://github.com/twistapps/modula/commit/57163d91e4fb4dc6417048d7a7a4e5ad2cc2eb6f))
* use new baseclass for MB ([d24ec93](https://github.com/twistapps/modula/commit/d24ec936c53d8239ab91c96f8e69851c6b9453db))


### Features

* auto bind DataLayer to templates by classname ([8e2a328](https://github.com/twistapps/modula/commit/8e2a328c12ce55cf511b86fc240d74dbc4f691fe))
* edit serialized props of modules from UI ([e640451](https://github.com/twistapps/modula/commit/e64045180b7a1cf602e5acad85e915f00be2a3ad))
* logging errors with Logger ([77f31e7](https://github.com/twistapps/modula/commit/77f31e704e1416e0e21cc4f5612b01b4d5353894))
* new attribute 'ForModules', works with multiple modules ([db0f728](https://github.com/twistapps/modula/commit/db0f72819a46de1849dc7a4704552246d0c266c8))
* New baseclass for MB-based components' editors ([c31d1a6](https://github.com/twistapps/modula/commit/c31d1a681450e2ec907942316767fb225e0f7439))
* show if component is missing in template ([14ba32d](https://github.com/twistapps/modula/commit/14ba32d64650e39a3af1614fd8dfdcf8fab9a9ff))

## [0.6.1](https://github.com/twistapps/modula/compare/0.6.0...0.6.1) (2022-04-20)


### Bug Fixes

* module duplicates are now being removed ([0bedecb](https://github.com/twistapps/modula/commit/0bedecb3df9ca3ed8a46ee1b0a1c21992f2a957f))

# [0.6.0](https://github.com/twistapps/modula/compare/0.5.1...0.6.0) (2022-04-20)


### Bug Fixes

* changing basepart now updates template GUI ([29e10ff](https://github.com/twistapps/modula/commit/29e10ff32f756df4434454ae60f53cd4a52a8606))
* extracted commonly used editor utils ([5ad6138](https://github.com/twistapps/modula/commit/5ad61387ca3fba0c451d05d4da372683fbb426cb))
* templates rework, improved stability ([997f9c3](https://github.com/twistapps/modula/commit/997f9c301ca6490d6eb2fc0af4e0d15ec44683e6))
* TypeNames occasionally throwing NullReferenceException ([dc904d4](https://github.com/twistapps/modula/commit/dc904d41977c6285de6b21df59912620103dab72))


### Features

* bind custom DataLayer type to MB by naming ([1964cbe](https://github.com/twistapps/modula/commit/1964cbec905c6acc382ec79b68a0e3ad93c4fa70))
* silence the logging, new log message ([2f10ab4](https://github.com/twistapps/modula/commit/2f10ab42fefed335b0ea257a31744417ab21565d))

## [0.5.1](https://github.com/twistapps/modula/compare/0.5.0...0.5.1) (2022-04-19)


### Bug Fixes

* systematization of code structure ([591bd48](https://github.com/twistapps/modula/commit/591bd48b88ce984de6ba236d4c47d31475029e96))

# [0.5.0](https://github.com/twistapps/modula/compare/0.4.0...0.5.0) (2022-03-17)


### Features

* actual module management via template system ([536a348](https://github.com/twistapps/modula/commit/536a348baeef6f352c10d5556d6b49a616f08d35))

# [0.4.0](https://github.com/twistapps/modula/compare/0.3.1...0.4.0) (2022-03-17)


### Features

* Examples for Template System ([#6](https://github.com/twistapps/modula/issues/6)) ([0beaf32](https://github.com/twistapps/modula/commit/0beaf327a3c461de4e01ae39d6959b1ef78d22dd))
* new utilities (ModulaUtilities, TypeNames) ([19165f0](https://github.com/twistapps/modula/commit/19165f06bfa133087061b50f02046493f2ccd4a7))
* Template System ([#6](https://github.com/twistapps/modula/issues/6)) ([58c5d92](https://github.com/twistapps/modula/commit/58c5d9257162f912b9e5ac4c865f7e087b530278))

## [0.3.1](https://github.com/twistapps/modula/compare/0.3.0...0.3.1) (2022-03-17)


### Bug Fixes

* rename ModuleUpdate -> ManagedUpdate ([4a68545](https://github.com/twistapps/modula/commit/4a68545d40c59136899990da893f4ca896d16cfb))

# [0.3.0](https://github.com/twistapps/modula/compare/0.2.1...0.3.0) (2022-03-17)


### Bug Fixes

* replacing TypeList with TypedList ([6ad6b9a](https://github.com/twistapps/modula/commit/6ad6b9adca158e938c942593fbc671bce547feb7))


### Features

* resolve dependencies that are not present ([4a18026](https://github.com/twistapps/modula/commit/4a180261de4c14625fad52b817f5799047990f79))
* TypedList: better typelist ([6158e1a](https://github.com/twistapps/modula/commit/6158e1a30eedae14ec0544fa30d4381b4f625860))

## [0.2.1](https://github.com/twistapps/modula/compare/0.2.0...0.2.1) (2022-03-11)


### Bug Fixes

* rename Parent > Main and fix ref in Module.cs ([ded4ab1](https://github.com/twistapps/modula/commit/ded4ab1ee49aa9e690a44a833d09929903a8b0f1))

# [0.2.0](https://github.com/twistapps/modula/compare/v0.1.0...0.2.0) (2022-03-10)


### Features

* register undo actions for module add/remove ([ec0bffd](https://github.com/twistapps/modula/commit/ec0bffdd746427b34fc37ac17b9cfc8f4244486b))
* some fields are back for Unity 2021 ([e601424](https://github.com/twistapps/modula/commit/e601424c9745c078d61438811c4349fc6d8601cf))
