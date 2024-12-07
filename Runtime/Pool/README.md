## POOL 

> Version `1.0.0` 

> Update 2024-12-05 17:33:20

> by nmd
----

#### Document:
- Open `MJGame Editor` --> `Pool` (check `PoolConfig.asset`)
- Create `PoolConfig.asset` to `Resources` folder
- Add item to `PoolConfig`: <*name*/*prefab*>
- Call Pool: 

```Csharp
SingletonComponent<PoolManager>.Instance.GetObject("<name>");
```

- Example: MJGame/Sample~/Pool