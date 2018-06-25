# 集中化配置方案：

* * *

* 配置信息存放于Github(Gitlab)上，可利用现有的可视化页面，可方便地查看修改记录，便于集中管理；
* 提供配置加载器基类，通过不同类型子类实现（目前实现了github）来对应不同的配置信息数据源；
* 提供类标注ConfigSetAttribute及字段标注ConfigItemAttribute，灵活管理配置信息类更新的方式；
* 根据标注信息，项目初始化时通过配置加载器实现的Load方法即可实现配置信息的按需管理；

* * *

具体使用方法参考以下链接说明：
https://www.cnblogs.com/you-you-111/p/9223982.html
