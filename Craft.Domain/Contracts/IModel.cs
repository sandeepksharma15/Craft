﻿namespace Craft.Domain.Contracts;

public interface IModel<TKey> : IHasId<TKey>;

public interface IModel : IModel<KeyType>;
