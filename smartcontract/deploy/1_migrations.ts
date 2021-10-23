module.exports = async({getNamedAccounts, deployments}) => {
    const {deploy} = deployments;
    const {deployer} = await getNamedAccounts();

    const GTinstance = await deploy('GalaxyToken', {
        from: deployer,
        log: true,
      });

    await deploy('SpaceMafia', {
      from: deployer,
      args: [GTinstance.address],
      log: true,
    });
  };
  module.exports.tags = ['All'];